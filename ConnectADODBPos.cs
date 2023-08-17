using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TALHA_PROJECTS_PRACTICE
{
    internal class ConnectADODBPos
    {
        public static void MainSyed()
        {
            TalhaConnectionString.Connect();
        }
    }
    public class TalhaConnectionString
    {
        public static ADODB.Connection DataBaseConnection;

        public static void Connect()
        {
            DataBaseConnection = new ADODB.Connection();
            DataBaseConnection.Provider = "SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Csharp;Data Source=desktop-5thqgm6\\sqlexpress";
            DataBaseConnection.Open();
            DataBaseConnection.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
        }
    }
}
