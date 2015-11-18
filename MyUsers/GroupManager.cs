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

            _strSQL = "SELECT * FROM GRUPPO  WHERE (1=1) " ;
            
            if (model.filter != null && !String.IsNullOrEmpty (model.filter.tipo )){
                _strSQL += " AND tipo_id = '" + model.filter.tipo  + "'";
            }

            if (model.hideGroupAdministrators)
            {
                _strSQL += " AND nome <> 'Administrators'";

            }

            _strSQL += " order by nome";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new Models.MyGroup(row));
            }


            model.Gruppi = risultato;
            model.TotalRows = risultato.Count();
        }



        public long countUsers(long gruppoId)
        {
            _strSQL = "SELECT COUNT(*) FROM UtenteGruppo  WHERE  gruppo_id = " + gruppoId;
            return long.Parse(_executeScalar(_strSQL));
        }

        public long countRoles(long gruppoId)
        {
            _strSQL = "SELECT COUNT(*) FROM GruppoRuolo  WHERE  gruppo_id = " + gruppoId;
            return long.Parse(_executeScalar(_strSQL));
        }


        public long insert(Models.MyGroup g)
        {
            long newId = -1;
            string strSQLParametri = "";

            _strSQL = "INSERT INTO GRUPPO ( nome , date_added ";
            strSQLParametri = " VALUES ( @nome , GetDate()  ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@nome", g.nome);


            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True Then
            //    Me._transactionBegin()
            //    _executeNoQuery(command)
            //    Me._transactionRollback()
            //    Return -1
            //End If

            _executeNoQuery(command);

            newId = _getIdentity();

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
            _strSQL = "INSERT INTO UtenteGruppo ( date_added,  gruppo_id, user_id ) VALUES ( GetDate() , " + gruppoId + "," + userId + ")";

            _executeNoQuery(_strSQL);
            return true;
        }


        public bool update(IEnumerable<Models.MyGroup> gruppi, long userId)
        {
            _strSQL = "DELETE FROM  UtenteGruppo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);

            foreach (Models.MyGroup g in gruppi)
            {
                addUser(g.gruppoId, userId);
            }

            return true;
        }


        public List<Models.MyGroup> getMyGroupFromRole(long userId, string ruoloId)
        {
            _strSQL = "select t4.* " +
                " from GruppoRuolo as t1 " +
                " join Ruolo as t2 on t1.ruolo_id = t2.ruolo_id " +
                " join UtenteGruppo as t3 on t1.gruppo_id = t3.gruppo_id " +
                " join Gruppo as t4 on t3.gruppo_id = t4.gruppo_id " +
                " where t2.ruolo_id = '" + ruoloId + "'and t3.user_id = " + userId;

            _dt = _fillDataTable(_strSQL);

            List<Models.MyGroup> risultato = new List<Models.MyGroup>();
            Models.MyGroup group;

            foreach (System.Data.DataRow row in _dt.Rows)
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
            _strSQL = "SELECT GRUPPO_ID FROM  GRUPPO WHERE UPPER(NOME)= @NOME ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@NOME", groupName.ToUpper().Trim());

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (_dt.Rows.Count == 0)
            {
                return -1;
            }

            return long.Parse(_dt.Rows[0]["GRUPPO_ID"].ToString());
        }

        public Models.MyGroup getGroup(long id)
        {
            _strSQL = "SELECT * FROM gruppo WHERE gruppo_id = " + id;

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            return new Models.MyGroup(_dt.Rows[0]);
        }


        public bool update(Models.MyGroup g)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _strSQL = "UPDATE GRUPPO SET date_modified = GetDate() , nome = @NOME  ";
            _addParameter(command, "@NOME", g.nome);

            _strSQL += ", tipo_id = @TIPO ";
            _addParameter(command, "@TIPO", g.tipo);
            

            _strSQL += " WHERE gruppo_id = " + g.gruppoId;
            command.CommandText = _strSQL;

            _executeNoQuery(command);
            return true;
        }






        public bool updateRuoli(Models.MyGroup g)
        {
            _strSQL = "DELETE FROM  GruppoRuolo WHERE gruppo_id = " + g.gruppoId;
            _executeNoQuery(_strSQL);

            foreach (Models.MyRole p in g.Ruoli)
            {
                insertRuolo(g.gruppoId, p.ruoloId);
            }

            return true; ;
        }


        private bool insertRuolo(long gruppoId, string ruoloId)
        {
            _strSQL = "INSERT INTO GruppoRuolo ( gruppo_id, ruolo_id )" +
                        " VALUES ( " + gruppoId + ",'" + ruoloId + "')";

            _executeNoQuery(_strSQL);

            return true;
        }



        public bool delete(long gruppoId)
        {
            _strSQL = "DELETE FROM UtenteGruppo WHERE gruppo_id = " + gruppoId;
            _executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM GruppoRuolo WHERE gruppo_id = " + gruppoId;
            _executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM Gruppo WHERE gruppo_id = " + gruppoId;
            _executeNoQuery(_strSQL);
            return true;
        }


        public bool deleteUser(long gruppoId, long usertId)
        {
            _strSQL = "DELETE FROM UtenteGruppo WHERE gruppo_id = " + gruppoId + " AND user_id = " + usertId;
            _executeNoQuery(_strSQL);

            return true;
        }



       

        public bool setGroups(Models.MyUser u)
        {
            _strSQL = "select t2.* from UtenteGruppo as t1 left join gruppo  as t2 on (t1.gruppo_id = t2.gruppo_id) WHERE t1.user_id = " + u.userId;


            //if (hideAdministrators)
            //{
            //    _strSQL += " AND T2.NOME <> 'Administrators' ";

            //}

            _dt = _fillDataTable(_strSQL);

            List<Models.MyGroup> listaGruppi = new List<Models.MyGroup>();
            Models.MyGroup g;

            foreach (DataRow row in _dt.Rows)
            {
                g = new Models.MyGroup(row);
                listaGruppi.Add(g);
            }

            u.Gruppi = listaGruppi;
            return true;
        }


        public List<Models.MyUser> getUsers(long gruppoId)
        {
            _strSQL = UserManager._sqlElencoUtenti + " from utente as t1 left join  UtenteGruppo  as t2 on (t1.user_id = t2.user_id) WHERE t2.gruppo_id = " + gruppoId;
            _dt = _fillDataTable(_strSQL);


            List<Models.MyUser> listaUtenti = new List<Models.MyUser>();
            Models.MyUser u;

            foreach (DataRow row in _dt.Rows)
            {
                u = new Models.MyUser(row, Models.MyUser.SelectFileds.Lista);
                //u = new Models.MyUser(row);
                listaUtenti.Add(u);
            }

            return listaUtenti;

        }

        public void setRoles(Models.MyGroup g)
        {
            _strSQL = "select t2.ruolo_id as ruolo_id  ,t2.nome as nome   from gruppoRuolo as t1 left join Ruolo as t2  on t1.ruolo_id  = t2.ruolo_id  WHERE t1.gruppo_id = " + g.gruppoId + " order by t2.nome  ";

            _dt = _fillDataTable(_strSQL);

            List<Models.MyRole> risultato = new List<Models.MyRole>();
            Models.MyRole p;

            foreach (DataRow row in _dt.Rows)
            {
                p = new Models.MyRole(row);

                risultato.Add(p);
            }

            g.Ruoli = risultato;
        }


        public bool setRoles(Models.MyUser u)
        {
            _strSQL = "select t4.ruolo_id , t4.nome ,  t5.gruppo_id , t5.nome as gruppo  " +
                " from Utente as t1 " +
                " join UtenteGruppo as t2 on (t1.user_id = t2.user_id ) " +
                " join GruppoRuolo as t3 on (t2.gruppo_id = t3.gruppo_id) " +
                " join Ruolo as t4 on (t3.ruolo_id = t4.ruolo_id) " +
                " join Gruppo as t5 on t3.gruppo_id = t5.gruppo_id " +
                " where t1.user_id = " + u.userId;


            _dt = _fillDataTable(_strSQL);

            List<Models.MyRole> risultato = new List<Models.MyRole>();
            Models.MyRole p;

            foreach (DataRow row in _dt.Rows)
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

            _strSQL = "SELECT * FROM RUOLO order by nome";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new Models.MyRole(row));
            }

            return risultato;
        }

        public List<MyManagerCSharp.Models.MyItem> getTypeList()
        {

            List<MyManagerCSharp.Models.MyItem> risultato = new List<MyManagerCSharp.Models.MyItem>();

            _strSQL = "SELECT * FROM GruppoTipo order by nome";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Models.MyItem(row["id"].ToString(), row["nome"].ToString()));
            }

            return risultato;
        }

    }
}
