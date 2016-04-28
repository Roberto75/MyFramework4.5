using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;

namespace MyUsers.Models
{

    //[TypeConverter(typeof(MyGroupConverter))]
    //[NotMapped]
    public class MyGroup
    {
        [Key]
        public long gruppoId { get; set; }
        public string nome { get; set; }
        public DateTime dateAdded { get; set; }
        public string tipo { get; set; }

        public long countUsers { get; set; }
        public long countRoles { get; set; }

        //public List<MyUser> utenti { get; set; }
        //public virtual ICollection<MyTest> Tests { get; set; }

        //public virtual ICollection<MyUser> Utenti { get; set; }

        public virtual ICollection<MyRole> Ruoli { get; set; }

        public MyGroup()
        {
            //Utenti = new HashSet<MyUser>();
            dateAdded = DateTime.Now;
        }


        public MyGroup(int id)
        {
            gruppoId = id;
        }

        public MyGroup(System.Data.DataRow row)
        {
            gruppoId = int.Parse(row["gruppo_id"].ToString());
            nome = row["nome"].ToString();
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            tipo = (row["tipo_id"] is DBNull) ? "" : row["tipo_id"].ToString();
        }
    }


    public class MyGroupConverter : System.ComponentModel.TypeConverter
    {
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {

            if (sourceType == typeof(string))
            {
                return true;
            }

            bool esito;
            esito = base.CanConvertFrom(context, sourceType);
            return esito;
        }
        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] v = ((string)value).Split(new char[] { ',' });

                MyGroup g = new MyGroup();
                g.gruppoId = int.Parse(v[0]);


                // if (v[1] != null)
                //{
                //    g.nome = v[1];
                //}

                return g;
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {

                string temp;
                temp = ((MyGroup)value).gruppoId + "," + ((MyGroup)value).nome;

                //return ((Point)value).X + "," + ((Point)value).Y;
                return temp;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }



        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }



        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }


        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }
    }



}
