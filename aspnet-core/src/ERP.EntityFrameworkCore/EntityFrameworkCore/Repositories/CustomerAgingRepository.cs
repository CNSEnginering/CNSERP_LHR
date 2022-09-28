using Abp.Data;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ERP.EntityFrameworkCore.Repositories
{
    public class CustomerAgingRepository: ERPRepositoryBase<GLTRHeader>
    {

        private readonly IActiveTransactionProvider _transactionProvider;

        public CustomerAgingRepository(IDbContextProvider<ERPDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public async Task<List<CustomerAgingDto>> GetReportData(CustomerAgingInputs inputs)
        {
            await EnsureConnectionOpenAsync();

            SqlParameter[] parameter = {
              new SqlParameter("@asofdate",SqlDbType.VarChar,10){ Value = inputs.asOnDate },
              new SqlParameter("@ACCID1",SqlDbType.VarChar,10){ Value = inputs.fromAccount },
              new SqlParameter("@ACCID2",SqlDbType.VarChar,10){ Value = inputs.toAccount },
              new SqlParameter("@SUBID1",SqlDbType.VarChar,10){ Value = inputs.frmSubAcc },
              new SqlParameter("@SUBID2",SqlDbType.VarChar,10){ Value = inputs.toSubAcc },
              new SqlParameter("@AGPRD",SqlDbType.VarChar,10){ Value = inputs.agingPeriod1 },
              new SqlParameter("@TENANT",SqlDbType.VarChar,10){ Value = inputs.TenantId },

};

            using (var command = CreateCommand("sp_glaging", CommandType.StoredProcedure, parameter))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var result = new List<CustomerAgingDto>();

                    while (dataReader.Read())
                    {
                        decimal TotalAmount;
                        decimal.TryParse(dataReader["TOTAL_AMOUNT"].ToString(), out TotalAmount);

                        decimal dueAmount;
                        decimal.TryParse(dataReader["due"].ToString(), out dueAmount);

                        decimal Amount0_30;
                        decimal.TryParse(dataReader["0-30"].ToString(), out Amount0_30);

                        decimal Amount31_60;
                        decimal.TryParse(dataReader["31-60"].ToString(), out Amount31_60);

                        decimal Amount61_90;
                        decimal.TryParse(dataReader["61-90"].ToString(), out Amount61_90);

                        decimal AboveAmount;
                        decimal.TryParse(dataReader["90 and above"].ToString(), out AboveAmount);


                        result.Add( new CustomerAgingDto {
                            subID = int.Parse(dataReader["SubAccID"].ToString()),
                            CustomerName = dataReader["SubAccName"].ToString(),
                            TotalAmount = TotalAmount,
                            dueAmount = dueAmount,
                            Amount0_30 = Amount0_30,
                            Amount31_60 = Amount31_60,
                            Amount61_90 = Amount61_90,
                            AboveAmount = AboveAmount

                        });
                    }

                    return result;
                }
            }
        }

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private async Task EnsureConnectionOpenAsync()
        {
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
    {
        {"ContextType", typeof(ERPDbContext) },
        {"MultiTenancySide", MultiTenancySide }
    });
        }

    }
}
