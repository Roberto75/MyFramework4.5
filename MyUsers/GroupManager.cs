using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MyUsers
{
    public class GroupManager : MyManagerCSharp.ManagerDB
    {
        public GroupManager(string connectionName)
            : base(connectionName)
        {

        }

        public GroupManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public List<Models.MyGroup> getListWithoutAdministrators()
        {
            MyUsers.Models.SearchGroups model = new Models.SearchGroups();
            model.hideGroupAdministrators = true;

            getList(model);

            return model.Gruppi;
        }

        public List<Models.MyGroup> getList()
        {
            MyUsers.Models.SearchGroups model = new Models.SearchGroups();

            getList(model);

            return model.Gruppi;
        }


        public void getList(MyUsers.Models.SearchGroups model)
        {
            List<Models.MyGroup> risultato;
            risultato = new List<Models.MyGroup>();

            mStrSQL = "SELECT * FROM GRUPPO  WHERE (1=1) " ;
            
            if (model.filter != null && !String.IsNullOrEmpty (model.filter.tipo )){
                mStrSQL += " AND tipo_id = '" + model.filter.tipo  + "'";
            }

            if (model.hideGroupAdministrators)
            {
                mStrSQL += " AND nome <> 'Administrators'";

            }

            mStrSQL += " order by nome";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.MyGroup(row));
            }


            model.Gruppi = risultato;
            model.TotalRows = risultato.Count();
        }



        public long countUsers(long gruppoId)
        {
            mStrSQL = "SELECT COUNT(*) FROM UtenteGruppo  WHERE  gruppo_id = " + gruppoId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }

        public long countRoles(long gruppoId)
        {
            mStrSQL = "SELECT COUNT(*) FROM GruppoRuolo  WHERE  gruppo_id = " + gruppoId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }


        public long insert(Models.MyGroup g)
        {
            long newId = -1;
            string strSQLParametri = "";

            mStrSQL = "INSERT INTO GRUPPO ( nome , date_added ";
            strSQLParametri = " VALUES ( @nome , GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mAddParameter(command, "@nome", g.nome);


            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True Then
            //    Me.mTransactionBegin()
            //    mExecuteNoQuery(command)
            //    Me.mTransactionRollback()
            //    Return -1
            //End If

            mExecuteNoQuery(command);

            newId = mGetIdentity();

            return newId;
        }


        public bool addUser(IEnumerable<Models.MyGroup> gruppi, long userId)
        {
            foreach (Models.MyGroup g in gruppi)
            {
                addUser(g.gruppoId, userId);
            }

            return true;
        }


        public bool addUser(long gruppoId, long userId)
        {
            mStrSQL = "INSERT INTO UtenteGruppo ( date_added,  gruppo_id, user_id ) VALUES ( GetDate() , " + gruppoId + "," + userId + ")";

            mExecuteNoQuery(mStrSQL);
            return true;
        }


        public bool update(IEnumerable<Models.MyGroup> gruppi, long userId)
        {
            mStrSQL = "DELETE FROM  UtenteGruppo WHERE user_id = " + userId;
            mExecuteNoQuery(mStrSQL);

            foreach (Models.MyGroup g in gruppi)
            {
                addUser(g.gruppoId, userId);
            }

            return true;
        }


        public List<Models.MyGroup> getMyGroupFromRole(long userId, string ruoloId)
        {
            mStrSQL = "select t4.* " +
                " from GruppoRuolo as t1 " +
                " join Ruolo as t2 on t1.ruolo_id = t2.ruolo_id " +
                " join UtenteGruppo as t3 on t1.gruppo_id = t3.gruppo_id " +
                " join Gruppo as t4 on t3.gruppo_id = t4.gruppo_id " +
                " where t2.ruolo_id = '" + ruoloId + "'and t3.user_id = " + userId;

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyGroup> risultato = new List<Models.MyGroup>();
            Models.MyGroup group;

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                //Escludo il gluppo administrators
                if (row["nome"].ToString().ToUpper() == "ADMINISTRATORS")
                {
                    continue;
                }

                group = new Models.MyGroup(row);
                risultato.Add(group);
            }

            return risultato;

        }


        public long getGroupIdFromName(string groupName)
        {
            mStrSQL = "SELECT GRUPPO_ID FROM  GRUPPO WHERE UPPER(NOME)= @NOME ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mAddParameter(command, "@NOME", groupName.ToUpper().Trim());

            command.CommandText = mStrSQL;
            mDt = mFillDataTable(command);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (mDt.Rows.Count == 0)
            {
                return -1;
            }

            return long.Parse(mDt.Rows[0]["GRUPPO_ID"].ToString());
        }

        public Models.MyGroup getGroup(long id)
        {
            mStrSQL = "SELECT * FROM gruppo WHERE gruppo_id = " + id;

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            return new Models.MyGroup(mDt.Rows[0]);
        }


        public bool update(Models.MyGroup g)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mStrSQL = "UPDATE GRUPPO SET date_modified = GetDate() , nome = @NOME  ";
            mAddParameter(command, "@NOME", g.nome);

            mStrSQL += ", tipo_id = @TIPO ";
            mAddParameter(command, "@TIPO", g.tipo);
            

            mStrSQL += " WHERE gruppo_id = " + g.gruppoId;
            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);
            return true;
        }






        public bool updateRuoli(Models.MyGroup g)
        {
            mStrSQL = "DELETE FROM  GruppoRuolo WHERE gruppo_id = " + g.gruppoId;
            mExecuteNoQuery(mStrSQL);

            foreach (Models.MyRole p in g.Ruoli)
            {
                insertRuolo(g.gruppoId, p.ruoloId);
            }

            return true; ;
        }


        private bool insertRuolo(long gruppoId, string ruoloId)
        {
            mStrSQL = "INSERT INTO GruppoRuolo ( gruppo_id, ruolo_id )" +
                        " VALUES ( " + gruppoId + ",'" + ruoloId + "')";

            mExecuteNoQuery(mStrSQL);

            return true;
        }



        public bool delete(long gruppoId)
        {
            mStrSQL = "DELETE FROM UtenteGruppo WHERE gruppo_id = " + gruppoId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE FROM GruppoRuolo WHERE gruppo_id = " + gruppoId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE FROM Gruppo WHERE gruppo_id = " + gruppoId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }


        public bool deleteUser(long gruppoId, long usertId)
        {
            mStrSQL = "DELETE FROM UtenteGruppo WHERE gruppo_id = " + gruppoId + " AND user_id = " + usertId;
            mExecuteNoQuery(mStrSQL);

            return true;
        }



       

        public bool setGroups(Models.MyUser u)
        {
            mStrSQL = "select t2.* from UtenteGruppo as t1 left join gruppo  as t2 on (t1.gruppo_id = t2.gruppo_id) WHERE t1.user_id = " + u.userId;


            //if (hideAdministrators)
            //{
            //    mStrSQL += " AND T2.NOME <> 'Administrators' ";

            //}

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyGroup> listaGruppi = new List<Models.MyGroup>();
            Models.MyGroup g;

            foreach (DataRow row in mDt.Rows)
            {
                g = new Models.MyGroup(row);
                listaGruppi.Add(g);
            }

            u.Gruppi = listaGruppi;
            return true;
        }


        public List<Models.MyUser> getUsers(long gruppoId)
        {
            mStrSQL = UserManager._sqlElencoUtenti + " from utente as t1 left join  UtenteGruppo  as t2 on (t1.user_id = t2.user_id) WHERE t2.gruppo_id = " + gruppoId;
            mDt = mFillDataTable(mStrSQL);


            List<Models.MyUser> listaUtenti = new List<Models.MyUser>();
            Models.MyUser u;

            foreach (DataRow row in mDt.Rows)
            {
                u = new Models.MyUser(row, Models.MyUser.SelectFileds.Lista);
                //u = new Models.MyUser(row);
                listaUtenti.Add(u);
            }

            return listaUtenti;

        }

        public void setRoles(Models.MyGroup g)
        {
            mStrSQL = "select t2.ruolo_id as ruolo_id  ,t2.nome as nome   from gruppoRuolo as t1 left join Ruolo as t2  on t1.ruolo_id  = t2.ruolo_id  WHERE t1.gruppo_id = " + g.gruppoId + " order by t2.nome  ";

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyRole> risultato = new List<Models.MyRole>();
            Models.MyRole p;

            foreach (DataRow row in mDt.Rows)
            {
                p = new Models.MyRole(row);

                risultato.Add(p);
            }

            g.Ruoli = risultato;
        }


        public bool setRoles(Models.MyUser u)
        {
            mStrSQL = "select t4.ruolo_id , t4.nome ,  t5.gruppo_id , t5.nome as gruppo  " +
                " from ((( Utente as t1 " +
                " left join UtenteGruppo as t2 on (t1.user_id = t2.user_id )) " +
                " left join GruppoRuolo as t3 on (t2.gruppo_id = t3.gruppo_id) )" +
                " left join Ruolo as t4 on (t3.ruolo_id = t4.ruolo_id) )" +
                " left join Gruppo as t5 on t3.gruppo_id = t5.gruppo_id " +
                " where t1.user_id = " + u.userId;
                        

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyRole> risultato = new List<Models.MyRole>();
            Models.MyRole p;

            foreach (DataRow row in mDt.Rows)
            {
                p = new Models.MyRole(row);

                risultato.Add(p);
            }

            u.Ruoli = risultato;

            return true;
        }



        public List<Models.MyRole> getRoleList()
        {

            List<Models.MyRole> risultato;
            risultato = new List<Models.MyRole>();

            mStrSQL = "SELECT * FROM RUOLO order by nome";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.MyRole(row));
            }

            return risultato;
        }

        public List<MyManagerCSharp.Models.MyItem> getTypeList()
        {

            List<MyManagerCSharp.Models.MyItem> risultato = new List<MyManagerCSharp.Models.MyItem>();

            mStrSQL = "SELECT * FROM GruppoTipo order by nome";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Models.MyItem(row["id"].ToString(), row["nome"].ToString()));
            }

            return risultato;
        }

    }
}
