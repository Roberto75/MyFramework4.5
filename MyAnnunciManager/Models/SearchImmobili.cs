using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Annunci.Models
{
    public class SearchImmobili : Pagedlmmobili
    {

        public enum EnumTempo
        {
            Oggi = 1,
            UltimaSettimana = 2,
            UltimoMese = 3,
            Tutti = 4
        }

        public enum EnumTipoAnnuncio
        {
            Agenzia,
            Privato
        }


        public enum EnumTipoVista
        {
            Elenco,
            Mappa
        }


        public Immobile filter { get; set; }
        public MyManagerCSharp.ManagerDB.Days? days { get; set; }

        public List<MyManagerCSharp.Models.MyItem> comboRegioni { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboProvince { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboComuni { get; set; }


        public EnumTempo Tempo { get; set; }
        public EnumTipoVista TipoVista { get; set; }
        public IList<EnumTipoAnnuncio> TipoAnnuncio { get; set; }
        //public IList<string> TipoAnnuncio { get; set; }

        //public bool? TipoAnnuncioAgenzia { get; set; }
        //public bool? TipoAnnuncioPrivato { get; set; }

        public int? prezzoMax { get; set; }
        public int? mqMin { get; set; }


        public SearchImmobili()
        {
            this.Sort = "ANNUNCIO.date_added";
            this.SortDir = "DESC";

            TipoAnnuncio = new List<EnumTipoAnnuncio>();
            TipoAnnuncio.Add(Models.SearchImmobili.EnumTipoAnnuncio.Privato);
            TipoAnnuncio.Add(Models.SearchImmobili.EnumTipoAnnuncio.Agenzia);

            //TipoAnnuncio = new List<string>();

            TipoVista = EnumTipoVista.Elenco;
            Tempo = EnumTempo.Tutti;

            prezzoMax = null;
            mqMin = null;


            comboProvince = new List<MyManagerCSharp.Models.MyItem>();
            comboComuni = new List<MyManagerCSharp.Models.MyItem>();

            filter = new Immobile();

        }

        public int countFilter()
        {
            int conta = 0;

            if (Tempo != EnumTempo.Tutti)
            {
                conta++;
            }

            /*if (!String.IsNullOrEmpty(filter.titolo))
            {
                conta++;
            }*/


            if (TipoAnnuncio != null && TipoAnnuncio.Count == 1)
            {

                conta++;
            }

            /*
            if (filter.categoriaId != null)
            {
                conta++;
            }

            if (filter.subCategoriaId != null)
            {
                conta++;
            }*/

            if (filter.regioneId != null)
            {
                conta++;
            }

            if (filter.provinciaId != null)
            {
                conta++;
            }

            if (filter.comuneId != null)
            {
                conta++;
            }
            return conta;
        }
    }
}
