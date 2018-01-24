using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Annunci.Models
{
    public class Annuncio
    {

        public enum SelectFileds
        {
            Lista,
            Full
        }

        public long annuncioId { get; set; }
        public DateTime dataInserimento { get; set; }
        public DateTime? dateDeleted { get; set; }
        public long customerId { get; set; }
        public long userId { get; set; }
        public string login { get; set; }


        public string nome { get; set; }

        [AllowHtml]
        public string nota { get; set; }


        public decimal prezzo { get; set; }


        public string regione { get; set; }
        public string provincia { get; set; }
        public string comune { get; set; }
        public int? regioneId { get; set; }
        public string provinciaId { get; set; }
        public string comuneId { get; set; }




        // le imposto a null altrimeti mi diventa un filtro obbligatorio nella RICERCA
        public Annunci.AnnunciManager.TipoAnnuncio? tipo { get; set; }

        public Annunci.AnnunciManager.StatoAnnuncio? stato { get; set; }

        public string categoria { get; set; }
        public int? categoriaId { get; set; }


        public DateTime dateLastClick { get; set; }
        public DateTime dateStartClickParziale { get; set; }
        public long countClick { get; set; }
        public long countClickParziale { get; set; }

        public Annuncio()
        {
            this.annuncioId = -1;
        }

        public Annuncio(System.Data.DataRow row, SelectFileds mode)
        {
            annuncioId = long.Parse(row["annuncio_id"].ToString());
            prezzo = (row["prezzo"] is DBNull) ? 0 : Decimal.Parse(row["prezzo"].ToString());

            dataInserimento = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            userId = (row["user_id"] is DBNull) ? 0 : long.Parse(row["user_id"].ToString());
            login = (row["my_login"] is DBNull) ? "" : row["my_login"].ToString();

            //            categoria = (Models.Immobile.Categorie)int.Parse(row["categoria_id"].ToString());

            // customerId = (row["customer_id"] is DBNull) ? -1 : long.Parse(row["customer_id"].ToString());
            nome = (row["nome"] is DBNull) ? "" : row["nome"].ToString();

            //Debug.WriteLine("Categoria: " + row["categoria"].ToString());
            //Debug.WriteLine("Tipo: " + row["tipo"].ToString());

            categoria = (row["categoria"] is DBNull) ? "" : row["categoria"].ToString();
            categoriaId = int.Parse(row["categoria_id"].ToString());

            if (!(row["tipo"] is DBNull))
            {
                tipo = (Annunci.AnnunciManager.TipoAnnuncio)int.Parse(row["tipo"].ToString());
            }
            else
            {
                Debug.WriteLine("Tipo IS NULL");
            }



            if (mode == SelectFileds.Full)
            {
                regione = (row["regione"] is DBNull) ? "" : row["regione"].ToString();
                provincia = (row["provincia"] is DBNull) ? "" : row["provincia"].ToString();
                comune = (row["comune"] is DBNull) ? "" : row["comune"].ToString();


                regioneId = (row["regione_id"] is DBNull) ? -1 : int.Parse(row["regione_id"].ToString());
                provinciaId = (row["provincia_id"] is DBNull) ? "" : row["provincia_id"].ToString();
                comuneId = (row["comune_id"] is DBNull) ? "" : row["comune_id"].ToString();


                nota = (row["DESCRIZIONE"] is DBNull) ? "" : row["DESCRIZIONE"].ToString();

                if (!(row["MY_STATO"] is DBNull))
                {
                    //stato = (Annunci.AnnunciManager.StatoAnnuncio)int.Parse(row["MY_STATO"].ToString());

                    if (row["MY_STATO"].ToString() == "OggettoNonPiuDisponibile")
                    {
                        stato = AnnunciManager.StatoAnnuncio.Oggetto_non_piu_disponibile;
                    }
                    else
                    {
                        stato = (Annunci.AnnunciManager.StatoAnnuncio)System.Enum.Parse(typeof(Annunci.AnnunciManager.StatoAnnuncio), row["MY_STATO"].ToString());
                    }


                }
                else
                {
                    Debug.WriteLine("MY_STATO IS NULL");
                }



                if (row["date_deleted"] is DBNull)
                {
                    dateDeleted = null;
                }else
                {
                    dateDeleted = DateTime.Parse(row["date_added"].ToString());
                }

                

                dateLastClick = (row["DATE_LAST_CLICK"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["DATE_LAST_CLICK"].ToString());
                dateStartClickParziale = (row["date_start_click_parziale"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_start_click_parziale"].ToString());
                countClick = long.Parse(row["COUNT_CLICK"].ToString());
                countClickParziale = long.Parse(row["COUNT_CLICK_PARZIALE"].ToString());
            }

        }
    }
}
