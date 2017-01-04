

namespace DataAccessLayer.DataModels.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ContextIdentifier
    {
    }

    class DynamicContextCreator
    {
        public static Type CreateMyNewType(string newTypeName, string propertyName, Type propertyType, Type baseClassType)
        {
            // create a dynamic assembly and module 
            AssemblyBuilder assemblyBldr =
            Thread.GetDomain().DefineDynamicAssembly(new AssemblyName("tmpAssembly"),
            AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBldr = assemblyBldr.DefineDynamicModule("tmpModule");

            // create a new type builder
            TypeBuilder typeBldr = moduleBldr.DefineType
            (newTypeName, TypeAttributes.Public | TypeAttributes.Class, baseClassType);

            // Generate a private field for the property
            FieldBuilder fldBldr = typeBldr.DefineField
            ("_" + propertyName, propertyType, FieldAttributes.Private);
            // Generate a public property
            PropertyBuilder prptyBldr =
                        typeBldr.DefineProperty(propertyName,
                PropertyAttributes.None, propertyType, new Type[] { propertyType });
            // The property set and property get methods need the following attributes:
            MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;
            // Define the "get" accessor method for newly created private field.
            MethodBuilder currGetPropMthdBldr =
                        typeBldr.DefineMethod("get_value", GetSetAttr, propertyType, null);

            // Intermediate Language stuff... as per Microsoft
            ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
            currGetIL.Emit(OpCodes.Ldarg_0);
            currGetIL.Emit(OpCodes.Ldfld, fldBldr);
            currGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for the newly created private field.
            MethodBuilder currSetPropMthdBldr = typeBldr.DefineMethod
            ("set_value", GetSetAttr, null, new Type[] { propertyType });

            // More Intermediate Language stuff...
            ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
            currSetIL.Emit(OpCodes.Ldarg_0);
            currSetIL.Emit(OpCodes.Ldarg_1);
            currSetIL.Emit(OpCodes.Stfld, fldBldr);
            currSetIL.Emit(OpCodes.Ret);
            // Assign the two methods created above to the PropertyBuilder's Set and Get
            prptyBldr.SetGetMethod(currGetPropMthdBldr);
            prptyBldr.SetSetMethod(currSetPropMthdBldr);
            // Generate (and deliver) my type
            return typeBldr.CreateType();
        }
    }
}
