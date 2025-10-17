using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Bases.WorkView.ApiRouter.ApiService.AssemblyGenerator
{
    public class AssemblyGenerator
    {
        public AssemblyGenerator(WorkViewApplication wvApp)
        {
            CreateAssembly(wvApp);
        } // initialized with null-forgiving to satisfy the compiler until CreateAssembly runs in ctor
        public PersistedAssemblyBuilder Assembly { get; set; } = null!;
        public ModuleBuilder Module { get; set; } = null!;
        public void SaveDll() => Save(true);

        public void CreateAssembly(WorkViewApplication wvApplication)
        {
            // validate input early to avoid nullable dereference issues
            string asmName = wvApplication?.Name?.Replace(" ", "") ?? throw new ArgumentNullException(nameof(wvApplication));
            AssemblyName myAsmName = new AssemblyName(asmName);

            Assembly = new PersistedAssemblyBuilder(myAsmName, typeof(object).Assembly);
            Module = Assembly.DefineDynamicModule(
                myAsmName.Name ?? asmName);

        }

        public Type CreateType(WorkViewClass workViewClass)
        {
            string typeName = workViewClass?.SystemName ?? throw new ArgumentNullException(nameof(workViewClass));
            TypeBuilder myType = Module.DefineType(
                typeName,
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(object));

            foreach (var attribute in workViewClass.Attributes)
            {
                PropertyBuilder pBuilder = CreateProperty(attribute, myType);
            }
            return myType.CreateType()!;
        }

        private static PropertyBuilder CreateProperty(WorkViewAttribute attribute, TypeBuilder tb)
        {
            if (attribute?.SystemName == null) throw new ArgumentNullException(nameof(attribute.SystemName));
            Type propType = attribute.AttributeType ?? typeof(object);

            // create a private backing field for the property
            FieldBuilder fbAttribute = tb.DefineField(
                $"_{attribute.SystemName.ToLower()}",
                propType,
                FieldAttributes.Private);

            // Define the property (no index parameters)
            PropertyBuilder pbAttribute = tb.DefineProperty(attribute.SystemName, PropertyAttributes.None, propType, null);

            MethodAttributes getSetAttr = MethodAttributes.Public |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            MethodBuilder mbGetAccessor = tb.DefineMethod(
                $"get_{attribute.SystemName}",
                getSetAttr,
                propType,
                Type.EmptyTypes);

            ILGenerator attributeGetIL = mbGetAccessor.GetILGenerator();
            attributeGetIL.Emit(OpCodes.Ldarg_0);
            attributeGetIL.Emit(OpCodes.Ldfld, fbAttribute);
            attributeGetIL.Emit(OpCodes.Ret);

            MethodBuilder mbSetAccessor = tb.DefineMethod(
                $"set_{attribute.SystemName}",
                getSetAttr,
                typeof(void),
                new Type[] { propType });

            ILGenerator attributeSetIL = mbSetAccessor.GetILGenerator();
            attributeSetIL.Emit(OpCodes.Ldarg_0);
            attributeSetIL.Emit(OpCodes.Ldarg_1);
            attributeSetIL.Emit(OpCodes.Stfld, fbAttribute);
            attributeSetIL.Emit(OpCodes.Ret);

            pbAttribute.SetGetMethod(mbGetAccessor);
            pbAttribute.SetSetMethod(mbSetAccessor);

            return pbAttribute;
        }

        private static FieldBuilder CreateField(WorkViewAttribute attribute, TypeBuilder tb)
        {
            Type fieldType = attribute.AttributeType ?? typeof(int);

            FieldBuilder fbAttribute = tb.DefineField(
            $"m_{(attribute.SystemName ?? "field").ToLower()}",
            fieldType,
            FieldAttributes.Private);

            Type[] parameterTypes = { fieldType };
            ConstructorBuilder ctor1 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();

            ctor1IL.Emit(OpCodes.Ldarg_0);
            ConstructorInfo? ci = typeof(object).GetConstructor(Type.EmptyTypes);
            ctor1IL.Emit(OpCodes.Call, ci!);
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbAttribute);
            ctor1IL.Emit(OpCodes.Ret);

            // Only provide a default int ctor if the field type is int
            if (fieldType == typeof(int))
            {
                ConstructorBuilder ctor0 = tb.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.Standard,
                    Type.EmptyTypes);

                ILGenerator ctor0IL = ctor0.GetILGenerator();
                ctor0IL.Emit(OpCodes.Ldarg_0);
                ctor0IL.Emit(OpCodes.Ldc_I4, 42);
                ctor0IL.Emit(OpCodes.Call, ctor1);
                ctor0IL.Emit(OpCodes.Ret);
            }

            return fbAttribute;
        }
        public void Save(bool emitDebugInfo)
        {
            try
            {
                string path = $"{Assembly.FullName}.dll";
                path = Path.GetFullPath(path);
                Console.WriteLine(path);
                Assembly.Save(path);
                return;

                MetadataBuilder metadataBuilder = Assembly.GenerateMetadata(out BlobBuilder ilStream, out _, out MetadataBuilder pdbBuilder);

                BlobBuilder portablePdbBlob = new BlobBuilder();
                PortablePdbBuilder portablePdbBuilder = new PortablePdbBuilder(pdbBuilder, metadataBuilder.GetRowCounts(), entryPoint: default);
                BlobContentId pdbContentId = portablePdbBuilder.Serialize(portablePdbBlob);
                using FileStream pdbFileStream = new FileStream($"{Assembly.FullName}.pdb", FileMode.Create, FileAccess.Write);
                portablePdbBlob.WriteContentTo(pdbFileStream);

                DebugDirectoryBuilder debugDirectoryBuilder = new DebugDirectoryBuilder();
                debugDirectoryBuilder.AddCodeViewEntry($"{Assembly.FullName}.pdb", pdbContentId, portablePdbBuilder.FormatVersion);

                ManagedPEBuilder peBuilder = new ManagedPEBuilder(
                                header: new PEHeaderBuilder(imageCharacteristics: Characteristics.ExecutableImage | Characteristics.Dll),
                                metadataRootBuilder: new MetadataRootBuilder(metadataBuilder),
                                ilStream: ilStream,
                                debugDirectoryBuilder: debugDirectoryBuilder);

                BlobBuilder peBlob = new BlobBuilder();
                peBuilder.Serialize(peBlob);
                using var dllFileStream = new FileStream($"{Assembly.FullName}.dll", FileMode.Create, FileAccess.Write);
                peBlob.WriteContentTo(dllFileStream);
                Console.WriteLine(dllFileStream.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}