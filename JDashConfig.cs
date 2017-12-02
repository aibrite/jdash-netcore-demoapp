using System;
using JDash.NetCore.Api;
using JDash.NetCore.Models;
using JDash.NetCore.Provider.MsSQL;
using JDash.NetCore.Provider.MySQL;
using Microsoft.AspNetCore.Http;

namespace jdash_netcore_demoapp
{
    public class JDashConfig : BaseJDashConfigurator
    {
        public JDashConfig(HttpContext context) : base(context)
        {
        }

        public override JDashPrincipal GetPrincipal()
        {
            return new JDashPrincipal("demo user");
        }

        public override IJDashProvider GetProvider()
        {
            // Ensure you have a valid database.
            string connectionString = "Your SQL Server connection string";
            return new JSQLProvider(connectionString);

            // if you are using MySql uncomment below lines.
            // string mySqlConnStr = "Server=127.0.0.1;Database=jdash;Uid=root;Pwd=1;";
            // return new JDash.NetCore.Provider.MySQL.JMySQLProvider(mySqlConnStr);
        }
    }

}