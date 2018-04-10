using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactAssignment.BLL
{
    public class DropBoxUser
    {
        //*** Define Class Properties
        public string AccountId{ get; set; }
        public string AccountCountry{ get; set; }
        public string AccountEmail{ get; set; }
        public bool AccountIsPaired{ get; set; }
        public string AccountLocale{ get; set; }
        public string  AccountDisplayName{ get; set; }
        public string  AccountFamiliarName{ get; set; }
        public string  AccountGivenName{ get; set; }
        public string  AccountSurname{ get; set; }
        public string  AccountReferralLink{ get; set; }

        //*** Team Info Properties
        public string   AccountTeamId{ get; set; }
        public string  AccountTeamName{ get; set; }
    }
}
