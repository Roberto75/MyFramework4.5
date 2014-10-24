using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers
{
    
    public class MyCustomIdentity : System.Security.Principal.IIdentity  //, System.Runtime.Serialization.ISerializable
    {

        private long  _id= -1;
        public long UserId { get { return _id; } private set  { _id = value ;} }

        public string Name { get; private set; }
        public string Login { get; private set; }
     
        public MyCustomIdentity(long id, string login)
        {
            this._id = id;
            this.Name = login;
            this.Login = login;
        }
        
        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return _id != -1; }
        }



        

        //#region ISerializable Members

        //public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        //{
        //    if (context.State == System.Runtime.Serialization.StreamingContextStates.CrossAppDomain)
        //    {
        //        MyCustomIndentity gIdent = new MyCustomIndentity( this.UserId ,this.Name);
        //        info.SetType(gIdent.GetType());

        //        System.Reflection.MemberInfo[] serializableMembers;
        //        object[] serializableValues;

        //        serializableMembers = System.Runtime.Serialization.FormatterServices.GetSerializableMembers(gIdent.GetType());
        //        serializableValues = System.Runtime.Serialization.FormatterServices.GetObjectData(gIdent, serializableMembers);

        //        for (int i = 0; i < serializableMembers.Length; i++)
        //        {
        //            info.AddValue(serializableMembers[i].Name, serializableValues[i]);
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("Serialization not supported");
        //    }
        //}

        //#endregion

    }
}
