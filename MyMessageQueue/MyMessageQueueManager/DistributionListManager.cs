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


        public bool exist(DistributionList list)
        {
            mStrSQL = "select count(*) from mmq.DistributionList  where UPPER(name) = @NAME ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mAddParameter(command, "@NAME", list.name.Trim());
            command.CommandText = mStrSQL;

            string risultato = mExecuteScalar(command);

            if (int.Parse(risultato) > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            return int.Parse(risultato) == 1;

        }


        public long createDistributionList(DistributionList list)
        {
            if (exist(list))
            {
                throw new ApplicationException("Esiste già una lista di distribuzione con questo nome: " + list.name ); 
            }




            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            mTransactionBegin();

            long newId = -1;
            try
            {
                mStrSQL = "INSERT INTO mmq.DistributionList ( date_added , name, nota, distribution_type ) VALUES ( GETDATE() , @NAME , @NOTA , " + (int)list.distributionType + ")";
                mAddParameter(command, "@NAME", list.name);
                mAddParameter(command, "@NOTA", list.nota);
                command.CommandText = mStrSQL;

                mExecuteNoQuery(command);

                newId = mGetIdentity();

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

                        mStrSQL = "INSERT INTO mmq.[DistributionListMembers] ( distribution_id , member_id , date_added ) VALUES (  " + newId + ", " + m.id + " ,  GetDate() )";
                        command.CommandText = mStrSQL;

                        mExecuteNoQuery(command);
                    }
                }


                 mTransactionCommit();
            }
            catch (Exception ex)
            {
                mTransactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                throw ex;
            }

            return newId;

        }

        public long insertMember(Member m)
        {
            long newId = -1;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();


            mStrSQL = "INSERT INTO mmq.Member ( date_added , name, email ) VALUES ( GETDATE() , @NAME , @EMAIL )";
            mAddParameter(command, "@NAME", m.name);
            mAddParameter(command, "@EMAIL", m.email);

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);

            newId = mGetIdentity();

            m.id = newId;

            return newId;
        }


        private bool existsMemeber(Member m)
        {
            mStrSQL = "select * from mmq.Member " +
               " where UPPER(name) = @NAME and UPPER(email) = @EMAIL ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mAddParameter(command, "@NAME", m.name.Trim());
            mAddParameter(command, "@EMAIL", m.email.Trim());

            command.CommandText = mStrSQL;

            mDt = mFillDataTable(command);

            if (mDt.Rows.Count == 0)
            {
                return false;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }
                        
            m.id = long.Parse(mDt.Rows[0]["id"].ToString());

            return true;
        }


        public bool insertMember(long distributionListId, string nome, string email)
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
                m.id = insertMember(m);
            }

            mStrSQL = "INSERT INTO mmq.[DistributionListMembers] ( distribution_id , member_id , date_added ) VALUES (  " + distributionListId + ", " + m.id + " ,  GetDate() )";
            mExecuteNoQuery(mStrSQL);

            return true;
        }


        public bool deleteMember(long distributionListId, long memberId)
        {
            mStrSQL = "DELETE FROM mmq.DistributionListMembers WHERE distribution_id = " + distributionListId + " and member_id = " + memberId;
            return mExecuteNoQuery(mStrSQL) == 1;
        }




        public DistributionList getDistributionList(long id)
        {
            mStrSQL = "SELECT * FROM mmq.[DistributionList] WHERE id = " + id;
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            DistributionList list;
            list = new DistributionList(mDt.Rows[0]);

            setMembers(list);

            return list;
        }

        public void setMembers(DistributionList list)
        {
            mStrSQL = "select t2.* " +
                " from mmq.[DistributionListMembers] as t1 " +
                " join mmq.Member as t2 on t1.member_id = t2.id " +
                " where distribution_id = " + list.id +
                " order by t2.name";

            mDt = mFillDataTable(mStrSQL);
                       

            if (list.Members == null)
            {
                list.Members = new List<Member>();
            }

            if (mDt.Rows.Count == 0)
            {
                return;
            }

            Member member;
            foreach (DataRow row in mDt.Rows)
            {
                member = new Member(row);
                list.Members.Add(member);
            }
        }

        public List<DistributionList> getList()
        {
            List<DistributionList> risultato = new List<DistributionList>();


            mStrSQL = "SELECT * FROM mmq.DistributionList order by name";
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return risultato;
            }



            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new DistributionList(row));
            }


            return risultato;
        }

    }
}
