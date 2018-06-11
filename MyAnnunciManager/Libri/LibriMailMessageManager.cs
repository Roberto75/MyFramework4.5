using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci.Libri
{
    public class LibriMailMessageManager : AnnunciMailMessageManager {
        public LibriMailMessageManager(string applicationName, string http) 
            :base(applicationName, http)
        {

            mIV = LibriSecurityManager.IV;
            mKey = LibriSecurityManager.Key;
        }


       


    }
}
