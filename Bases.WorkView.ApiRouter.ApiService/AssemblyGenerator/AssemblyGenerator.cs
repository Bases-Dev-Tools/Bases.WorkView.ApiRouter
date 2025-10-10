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
        }
        public AssemblyBuilder Assembly {  get; set; }
        public ModuleBuilder Module { get; set; }
        public void SaveDll() => AssemblyGenerator.Save(Assembly, Assembly.GetName().Name, true);
        public void CreateAssembly(WorkViewApplication wvApplication)
        {
            AppDomain myDomain = AppDomain.CurrentDomain;
            AssemblyName myAsmName = new AssemblyName(wvApplication.Name.Replace(" ",""));
            AssemblyBuilder myAssembly = AssemblyBuilder.DefineDynamicAssembly(
                myAsmName,
                AssemblyBuilderAccess.Run);
            Assembly = myAssembly;
            

            ModuleBuilder myModule = myAssembly.DefineDynamicModule(
                myAsmName.Name);
            Module = myModule; 
            //Module.
        }

        public Type CreateType(WorkViewClass workViewClass)
        {
            TypeBuilder myType = Module.DefineType(
                workViewClass.SystemName,
                TypeAttributes.Public | TypeAttributes.Class);

            foreach (var attribute in workViewClass.Attributes)
            {
                PropertyBuilder pBuilder = CreateProperty(attribute, myType);
            }                        
            return myType.CreateType();           
        }

        private static PropertyBuilder CreateProperty(WorkViewAttribute attribute, TypeBuilder tb)
        {

            //var fbAttribute = CreateField(attribute, tb);
            PropertyBuilder pbAttribute = tb.DefineProperty(attribute.SystemName, PropertyAttributes.None, attribute.AttributeType, [attribute.AttributeType]);
            MethodAttributes getSetAttr = MethodAttributes.Public |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                $"get_{attribute.SystemName}",
                getSetAttr,
                attribute.AttributeType,
                Type.EmptyTypes);

            ILGenerator attributeGetIL = mbNumberGetAccessor.GetILGenerator();
            attributeGetIL.Emit(OpCodes.Ldarg_0);
            attributeGetIL.Emit(OpCodes.Ret);

            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_Number",
                getSetAttr,
                null,
                new Type[] { attribute.AttributeType });

            ILGenerator attributeSetIL = mbNumberSetAccessor.GetILGenerator();

            attributeSetIL.Emit(OpCodes.Ldarg_0);
            attributeSetIL.Emit(OpCodes.Ldarg_1);
            attributeSetIL.Emit(OpCodes.Ret);

            pbAttribute.SetGetMethod(mbNumberGetAccessor);
            pbAttribute.SetSetMethod(mbNumberSetAccessor);

            return pbAttribute;
        }

        private static FieldBuilder CreateField(WorkViewAttribute attribute, TypeBuilder tb)
        {
            FieldBuilder fbAttribute = tb.DefineField(
            $"m_{attribute.SystemName.ToLower()}",
            typeof(int),
            FieldAttributes.Private);

            Type[] parameterTypes = { attribute.AttributeType };
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

            ConstructorBuilder ctor0 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                Type.EmptyTypes);

            ILGenerator ctor0IL = ctor0.GetILGenerator();
            ctor0IL.Emit(OpCodes.Ldarg_0);
            ctor0IL.Emit(OpCodes.Ldc_I4_S, "42");
            ctor0IL.Emit(OpCodes.Call, ctor1);
            ctor0IL.Emit(OpCodes.Ret);

            return fbAttribute;
        }
        public static void Save(AssemblyBuilder ab, string assemblyFileName, bool emitDebugInfo)
        {
            try
            {
                PersistedAssemblyBuilder pb = new PersistedAssemblyBuilder(ab.GetName(), ab);
                MetadataBuilder metadataBuilder = pb.GenerateMetadata(out BlobBuilder ilStream, out _, out MetadataBuilder pdbBuilder);

                BlobBuilder portablePdbBlob = new BlobBuilder();
                PortablePdbBuilder portablePdbBuilder = new PortablePdbBuilder(pdbBuilder, metadataBuilder.GetRowCounts(), entryPoint: default);
                BlobContentId pdbContentId = portablePdbBuilder.Serialize(portablePdbBlob);
                using FileStream pdbFileStream = new FileStream($"{assemblyFileName}.pdb", FileMode.Create, FileAccess.Write);
                portablePdbBlob.WriteContentTo(pdbFileStream);

                DebugDirectoryBuilder debugDirectoryBuilder = new DebugDirectoryBuilder();
                debugDirectoryBuilder.AddCodeViewEntry($"{assemblyFileName}.pdb", pdbContentId, portablePdbBuilder.FormatVersion);

                ManagedPEBuilder peBuilder = new ManagedPEBuilder(
                                header: new PEHeaderBuilder(imageCharacteristics: Characteristics.ExecutableImage | Characteristics.Dll),
                                metadataRootBuilder: new MetadataRootBuilder(metadataBuilder),
                                ilStream: ilStream,
                                debugDirectoryBuilder: debugDirectoryBuilder);

                BlobBuilder peBlob = new BlobBuilder();
                peBuilder.Serialize(peBlob);
                using var dllFileStream = new FileStream($"{assemblyFileName}.dll", FileMode.Create, FileAccess.Write);
                peBlob.WriteContentTo(dllFileStream);
                Console.WriteLine(dllFileStream.Name);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }

}
