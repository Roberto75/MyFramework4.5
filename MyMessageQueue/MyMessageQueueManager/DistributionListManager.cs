using My.MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue
{
    public class DistributionListManager : MyManagerCSharp.ManagerDB
    {

        public DistributionListManager(string connectionName)
            : base(connectionName)
        {

        }

        public DistributionListManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public long createDistributionList(DistributionList list)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _transactionBegin();

            long newId = -1;

            try
            {

                _strSQL = "INSERT INTO mmq.DistributionList ( date_added , name, nota, distribution_type ) VALUES ( GETDATE() , @NAME , @NOTA , " + (int)list.distributionType + ")";
                _addParameter(command, "@NAME", list.name);
                _addParameter(command, "@NOTA", list.nota);
                command.CommandText = _strSQL;

                _executeNoQuery(command);

                newId = _getIdentity();


                //Members
                if (list.Members != null)
                {
                    command.Parameters.Clear();
                    foreach (Member m in list.Members)
                    {
                        if (m.id == 0)
                        {
                            insertMember(m);
                        }

                        _strSQL = "INSERT INTO mmq.[DistributionListMembers] ( distribution_id , member_id , date_added ) VALUES (  " + newId + ", " + m.id + " ,  GetDate() )";
                        command.CommandText = _strSQL;

                        _executeNoQuery(command);
                    }
                }


                _transactionCommit();
            }
            catch (Exception ex)
            {
                _transactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                throw ex;
            }

            return newId;

        }

        public long insertMember(Member m)
        {
            long newId = -1;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();


            _strSQL = "INSERT INTO mmq.Member ( date_added , name, email ) VALUES ( GETDATE() , @NAME , @EMAIL )";
            _addParameter(command, "@NAME", m.name);
            _addParameter(command, "@EMAIL", m.email);

            command.CommandText = _strSQL;

            _executeNoQuery(command);

            newId = _getIdentity();

            m.id = newId;

            return newId;
        }


        private bool existsMemeber(Member m)
        {
            _strSQL = "select count(*) from mmq.Member " +
               " where UPPER(name) = @NAME and UPPER(email) = @EMAIL ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@NAME", m.name.Trim());
            _addParameter(command, "@EMAIL", m.email.Trim());

            command.CommandText = _strSQL;

            string risultato = _executeScalar(command);

            if (int.Parse(risultato) > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            return int.Parse(risultato) == 1;
        }


        public bool  insertMember(long distributionListId, string nome, string email)
        {

            if (String.IsNullOrEmpty(nome))
            {
                throw new ArgumentNullException("Il nome dell'utente è un valore obbligatorio");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("L'indirizzo email dell'utente è un valore obbligatorio");
            }

            Member m = new Member(nome, email);

            if (!existsMemeber(m))
            {
               m.id =  insertMember(m);
            }
            
            _strSQL = "INSERT INTO mmq.[DistributionListMembers] ( distribution_id , member_id , date_added ) VALUES (  " + distributionListId + ", " + m.id + " ,  GetDate() )";
            _executeNoQuery(_strSQL);

            return true;
        }


        public bool deleteMember(long distributionListId, long memberId)
        {
            _strSQL = "DELETE FROM mmq.DistributionListMembers WHERE distribution_id = " + distributionListId + " and member_id = " + memberId;
            return _executeNoQuery(_strSQL) == 1;
        }




        public DistributionList getDistributionList(long id)
        {
            _strSQL = "SELECT * FROM mmq.[DistributionList] WHERE id = " + id;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            DistributionList list;
            list = new DistributionList(_dt.Rows[0]);

            setMembers(list);

            return list;
        }

        public void setMembers(DistributionList list)
        {
            _strSQL = "select t2.* " +
                " from mmq.[DistributionListMembers] as t1 " +
                " join mmq.Member as t2 on t1.member_id = t2.id " +
                " where distribution_id = " + list.id +
                " order by t2.name";

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return;
            }

            if (list.Members == null)
            {
                list.Members = new List<Member>();
            }



            Member member;
            foreach (DataRow row in _dt.Rows)
            {
                member = new Member(row);
                list.Members.Add(member);
            }
        }

        public List<DistributionList> getList()
        {

            _strSQL = "SELECT * FROM mmq.DistributionList order by name";
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            List<DistributionList> risultato = new List<DistributionList>();

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new DistributionList(row));
            }


            return risultato;
        }

    }
}
