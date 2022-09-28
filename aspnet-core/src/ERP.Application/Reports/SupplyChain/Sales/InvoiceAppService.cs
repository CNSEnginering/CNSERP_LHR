using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Sales.SaleEntry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Sales
{
    public class InvoiceAppService : ERPReportAppServiceBase, IInvoiceAppService
    {
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartOfAccRepository;
        private readonly IRepository<AccountSubLedger> _accountSubRepository;
        private readonly IRepository<GatePass> _gatePassRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<TransactionType> _transTypeRepository;
        public InvoiceAppService(IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<OESALEDetail> oesaleDetailRepository,
            IRepository<ChartofControl, string> chartOfAccRepository,
            IRepository<AccountSubLedger> accountSubRepository,
            IRepository<GatePass> gatePassRepository,
            IRepository<ICItem> itemRepository,
            IRepository<TransactionType> transTypeRepository
            )
        {
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _oesaleDetailRepository = oesaleDetailRepository;
            _chartOfAccRepository = chartOfAccRepository;
            _gatePassRepository = gatePassRepository;
            _accountSubRepository = accountSubRepository;
            _itemRepository = itemRepository;
            _transTypeRepository = transTypeRepository;
        }

        public List<InvoiceReport> GetData(int? tenantId,
          int fromDoc, int toDoc, DateTime fromDate, DateTime toDate)
        {
            tenantId = tenantId == 0 ? AbpSession.TenantId : tenantId;
            var data = from a in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocNo <= fromDoc && o.DocNo >= toDoc)
                       join
                       b in _oesaleDetailRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                       join
                       c in _chartOfAccRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.SalesCtrlAcc, B = a.TenantId } equals new { A = c.Id, B = c.TenantId }
                       join
                       d in _accountSubRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.SalesCtrlAcc, B = a.TenantId, C = Convert.ToInt32(a.CustID) } equals new { A = d.AccountID, B = d.TenantId, C = d.Id }
                       join
                       e in _gatePassRepository.GetAll().Where(o => o.TenantId == tenantId && o.TypeID == 2)
                       on new { A = a.OGP, B = a.TenantId } equals new { A = e.DocNo.ToString(), B = e.TenantId } into gatePass
                       from gatePassNew in gatePass.DefaultIfEmpty()
                       join
                       g in _itemRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = b.ItemID, B = b.TenantId } equals new { A = g.ItemId, B = g.TenantId }
                       join
                       h in _transTypeRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.TypeID, B = b.TenantId } equals new { A = h.TypeId, B = h.TenantId } into transType
                       from subTrans in transType.DefaultIfEmpty()
                       select new InvoiceReport()
                       {
                           DocNo = a.DocNo,
                           DocDate = (a.DocDate.Value.Day + "/" + a.DocDate.Value.Month + "/" + a.DocDate.Value.Year).ToString(),
                           OGPNo = a.OGP,
                           DriverName = gatePassNew.DriverName,
                           CustomerName = d.SubAccName,
                           Rate = b.Rate,
                           Qty = b.Qty,
                           ExlTaxAmount = b.ExlTaxAmount,
                           DeliveryCharges = a.Freight,
                           SalesMargin = a.Margin,
                           VehicleNo = gatePassNew.VehicleNo,
                           Unit = b.Unit,
                           Narration = a.Narration,
                           ItemId = b.ItemID,
                           Descp = g.Descp,
                           Disc = b.Disc,
                           Tax = b.TaxRate,
                           SalesTaxAmt = b.TaxAmt,
                           AdvIncTax = ((b.ExlTaxAmount) + (b.TaxAmt)) * 0.5 /100 ,
                           NetAmount = b.NetAmount,
                          // ValueExlSalesTax = b.Qty * b.Rate,
                           //SalesTaxAmt = (b.TaxRate / 100) * (b.Qty * b.Rate),
                           //AmtInclSalesTax = (b.Qty * b.Rate) + (b.TaxRate / 100) * (b.Qty * b.Rate),
                           TypeName = subTrans.Description == null ? "" : subTrans.Description,
                           CustId = a.CustID,
                           Amount = b.Amount
                       };
            return data.ToList();
        }

      

        public List<QuotationReport> GetQutationData(int? tenantId,
       int fromDoc, int toDoc)
        {
            tenantId = tenantId == 0 ? AbpSession.TenantId : tenantId;
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<QuotationReport> QuotationDtoList = new List<QuotationReport>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                cmd = new SqlCommand("sp_QutationMaster", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                var GrandAmt1 = 0.0;
                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@FromDocNo", fromDoc);
                cmd.Parameters.AddWithValue("@ToDocNo", toDoc);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        QuotationReport QutationData = new QuotationReport()
                        {
                            Id = rdr["Id"] is DBNull ? 0 : Convert.ToInt32(rdr["Id"]),
                            LocID = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
                            LocName = rdr["LocName"] is DBNull ? "" : rdr["LocName"].ToString(),
                            DocNo = rdr["DocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["DocNo"]),
                            DocDate = rdr["DocDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString()),
                            MDocDate = rdr["MDocDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(rdr["MDocDate"].ToString()),
                            MDocNo = rdr["MDocNo"] is DBNull ? "" : rdr["MDocNo"].ToString(),
                            TypeID = rdr["TypeID"] is DBNull ? "" : rdr["TypeID"].ToString(),
                            SalesCtrlAcc = rdr["SalesCtrlAcc"] is DBNull ? "" : rdr["SalesCtrlAcc"].ToString(),
                            CustID = rdr["CustID"] is DBNull ? 0 : Convert.ToInt32(rdr["CustID"]),
                            CustomerName = rdr["CustomerName"] is DBNull ? "" : rdr["CustomerName"].ToString(),
                            Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                            TransType = rdr["TransType"] is DBNull ? 0 : Convert.ToInt32(rdr["TransType"]),
                            transDESC = rdr["transDESC"] is DBNull ? "" : rdr["transDESC"].ToString(),
                            ItemID = rdr["ItemID"] is DBNull ? "" : rdr["ItemID"].ToString(),
                            ITEM_NAME = rdr["ITEM_NAME"] is DBNull ? "" : rdr["ITEM_NAME"].ToString(),
                            UNIT = rdr["UNIT"] is DBNull ? "" : rdr["UNIT"].ToString(),
                            Conver = rdr["Conver"] is DBNull ? "" : rdr["Conver"].ToString(),
                            Qty = rdr["Qty"] is DBNull ? 0.0 : Convert.ToDouble(rdr["Qty"].ToString()),
                            Rate = rdr["Rate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["Rate"].ToString()),
                            Amount = rdr["Amount"] is DBNull ? 0.0 : Convert.ToDouble(rdr["Amount"].ToString()),
                            TaxAuth = rdr["TaxAuth"] is DBNull ? "" : rdr["TaxAuth"].ToString(),
                            CLASSDESC = rdr["CLASSDESC"] is DBNull ? "" : rdr["CLASSDESC"].ToString(),
                            TaxRate = rdr["TaxRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxRate"].ToString()),
                            TaxAmt = rdr["TaxAmt"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxAmt"].ToString()),
                            NetAmount = rdr["NetAmount"] is DBNull ? 0.0 : Convert.ToDouble(rdr["NetAmount"].ToString()),
                            Remarks = rdr["Remarks"] is DBNull ? "" : rdr["Remarks"].ToString(),
                            NoteText = rdr["NoteText"] is DBNull ? "" : rdr["NoteText"].ToString(),
                            PayTerms = rdr["PayTerms"] is DBNull ? "" : rdr["PayTerms"].ToString(),
                            DelvTerms = rdr["DelvTerms"] is DBNull ? "" : rdr["DelvTerms"].ToString(),
                            ValidityTerms = rdr["ValidityTerms"] is DBNull ? "" : rdr["ValidityTerms"].ToString(),
                            CreatedBy = rdr["CreatedBy"] is DBNull ? "" : rdr["CreatedBy"].ToString(),

                            TaxAuth1 = rdr["TaxAuth1"] is DBNull ? "" : rdr["TaxAuth1"].ToString(),
                            TaxClass1 = rdr["TaxClass1"] is DBNull ? "" : rdr["TaxClass1"].ToString(),
                            TaxClassDesc1 = rdr["TaxClassDesc1"] is DBNull ? "" : rdr["TaxClassDesc1"].ToString(),
                            TaxRate1 = rdr["TaxRate1"] is DBNull ? "" : rdr["TaxRate1"].ToString() == "0" ? "" : rdr["TaxRate1"].ToString() + '%',
                            TaxAmt1 = rdr["TaxAmt1"] is DBNull ? "" : rdr["TaxAmt1"].ToString() == "0" ? "" : rdr["TaxAmt1"].ToString(),

                            TaxAuth2 = rdr["TaxAuth2"] is DBNull ? "" : rdr["TaxAuth2"].ToString(),
                            TaxClass2 = rdr["TaxClass2"] is DBNull ? "" : rdr["TaxClass2"].ToString(),
                            TaxClassDesc2 = rdr["TaxClassDesc2"] is DBNull ? "" : rdr["TaxClassDesc2"].ToString(),
                            TaxRate2 = rdr["TaxRate2"] is DBNull ? "" : rdr["TaxRate2"].ToString() == "0" ? "" : rdr["TaxRate2"].ToString() + '%',
                            TaxAmt2 = rdr["TaxAmt2"] is DBNull ? "" : rdr["TaxAmt2"].ToString() == "0" ? "" : rdr["TaxAmt2"].ToString(),

                            TaxAuth3 = rdr["TaxAuth3"] is DBNull ? "" : rdr["TaxAuth3"].ToString(),
                            TaxClass3 = rdr["TaxClass3"] is DBNull ? "" : rdr["TaxClass3"].ToString(),
                            TaxClassDesc3 = rdr["TaxClassDesc3"] is DBNull ? "" : rdr["TaxClassDesc3"].ToString(),
                            TaxRate3 = rdr["TaxRate3"] is DBNull ? "" : rdr["TaxRate3"].ToString() == "0" ? "" : rdr["TaxRate3"].ToString() + '%',
                            TaxAmt3 = rdr["TaxAmt3"] is DBNull ? "" : rdr["TaxAmt3"].ToString() == "0" ? "" : rdr["TaxAmt3"].ToString(),

                            TaxAuth4 = rdr["TaxAuth4"] is DBNull ? "" : rdr["TaxAuth4"].ToString(),
                            TaxClass4 = rdr["TaxClass4"] is DBNull ? "" : rdr["TaxClass4"].ToString(),
                            TaxClassDesc4 = rdr["TaxClassDesc4"] is DBNull ? "" : rdr["TaxClassDesc4"].ToString(),
                            TaxRate4 = rdr["TaxRate4"] is DBNull ? "" : rdr["TaxRate4"].ToString() == "0" ? "" : rdr["TaxRate4"].ToString() + '%',
                            TaxAmt4 = rdr["TaxAmt4"] is DBNull ? "" : rdr["TaxAmt4"].ToString() == "0" ? "" : rdr["TaxAmt4"].ToString(),

                            TaxAuth5 = rdr["TaxAuth5"] is DBNull ? "" : rdr["TaxAuth5"].ToString(),
                            TaxClass5 = rdr["TaxClass5"] is DBNull ? "" : rdr["TaxClass5"].ToString(),
                            TaxClassDesc5 = rdr["TaxClassDesc5"] is DBNull ? "" : rdr["TaxClassDesc5"].ToString(),
                            TaxRate5 = rdr["TaxRate5"] is DBNull ? "" : rdr["TaxRate5"].ToString() == "0" ? "" : rdr["TaxRate5"].ToString() + '%',
                            TaxAmt5 = rdr["TaxAmt5"] is DBNull ? "" : rdr["TaxAmt5"].ToString() == "0" ? "" : rdr["TaxAmt5"].ToString(),
                            Netamt = rdr["NetAmount"] is DBNull ? 0 : Convert.ToDouble(rdr["NetAmount"].ToString()) + Convert.ToDouble(rdr["TaxAmt1"].ToString()) + Convert.ToDouble(rdr["TaxAmt2"].ToString()) + Convert.ToDouble(rdr["TaxAmt3"].ToString()) + Convert.ToDouble(rdr["TaxAmt4"].ToString()) + Convert.ToDouble(rdr["TaxAmt5"].ToString()),

                        };

                        QuotationDtoList.Add(QutationData);

                    }

                }
            }
            return QuotationDtoList;

        }

        public List<CostSheetReport> GetCostsheetData(int? tenantId,
     int fromDoc, int toDoc)
        {
            tenantId = tenantId == 0 ? AbpSession.TenantId : tenantId;
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<CostSheetReport> CostSheetDtoList = new List<CostSheetReport>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                cmd = new SqlCommand("sp_CostSheet", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                var GrandAmt1 = 0.0;
                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@FromDocNo", fromDoc);
                cmd.Parameters.AddWithValue("@ToDocNo", toDoc);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        CostSheetReport CostSheetData = new CostSheetReport()
                        {
                            Id = rdr["Id"] is DBNull ? 0 : Convert.ToInt32(rdr["Id"]),
                            LocID = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
                            LocName = rdr["LocName"] is DBNull ? "" : rdr["LocName"].ToString(),
                            DocNo = rdr["DocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["DocNo"]),
                            SaleDoc = rdr["SaleDoc"] is DBNull ? 0 : Convert.ToInt32(rdr["SaleDoc"]),
                            DocDate = rdr["DocDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString()),
                            MDocDate = rdr["MDocDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(rdr["MDocDate"].ToString()),
                            MDocNo = rdr["MDocNo"] is DBNull ? "" : rdr["MDocNo"].ToString(),
                            TypeID = rdr["TypeID"] is DBNull ? "" : rdr["TypeID"].ToString(),
                            SalesCtrlAcc = rdr["SalesCtrlAcc"] is DBNull ? "" : rdr["SalesCtrlAcc"].ToString(),
                            CustID = rdr["CustID"] is DBNull ? 0 : Convert.ToInt32(rdr["CustID"]),
                            CustomerName = rdr["CustomerName"] is DBNull ? "" : rdr["CustomerName"].ToString(),
                            Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                            TransType = rdr["TransType"] is DBNull ? 0 : Convert.ToInt32(rdr["TransType"]),
                            transDESC = rdr["transDESC"] is DBNull ? "" : rdr["transDESC"].ToString(),
                            ItemID = rdr["ItemID"] is DBNull ? "" : rdr["ItemID"].ToString(),
                            ITEM_NAME = rdr["ITEM_NAME"] is DBNull ? "" : rdr["ITEM_NAME"].ToString(),
                            UNIT = rdr["UNIT"] is DBNull ? "" : rdr["UNIT"].ToString(),
                            Conver = rdr["Conver"] is DBNull ? "" : rdr["Conver"].ToString(),
                            Qty = rdr["Qty"] is DBNull ? 0.0 : Convert.ToDouble(rdr["Qty"].ToString()),
                            Rate = rdr["Rate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["Rate"].ToString()),
                            Amount = rdr["NetAmount"] is DBNull ? 0.0 : Convert.ToDouble(rdr["NetAmount"].ToString()),
                            TaxAuth = rdr["TaxAuth"] is DBNull ? "" : rdr["TaxAuth"].ToString(),
                            CLASSDESC = rdr["CLASSDESC"] is DBNull ? "" : rdr["CLASSDESC"].ToString(),
                            TaxRateD = rdr["TaxRateD"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxRateD"].ToString()),
                            TaxAmtD = rdr["TaxAmtD"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxAmtD"].ToString()),
                            Remarks = rdr["Remarks"] is DBNull ? "" : rdr["Remarks"].ToString(),
                            NoteText = rdr["NoteText"] is DBNull ? "" : rdr["NoteText"].ToString(),
                            PayTerms = rdr["PayTerms"] is DBNull ? "" : rdr["PayTerms"].ToString(),
                            DelvTerms = rdr["DelvTerms"] is DBNull ? "" : rdr["DelvTerms"].ToString(),
                            ValidityTerms = rdr["ValidityTerms"] is DBNull ? "" : rdr["ValidityTerms"].ToString(),
                            CreatedBy = rdr["CreatedBy"] is DBNull ? "" : rdr["CreatedBy"].ToString(),
                            BasicStyle = rdr["BasicStyle"] is DBNull ? "" : rdr["BasicStyle"].ToString(),
                            License = rdr["License"] is DBNull ? "" : rdr["License"].ToString(),
                            PartyName = rdr["PartyName"] is DBNull ? "" : rdr["PartyName"].ToString(),
                            ItemName = rdr["ItemName"] is DBNull ? "" : rdr["ItemName"].ToString(),
                            OrderQty = rdr["OrderQty"] is DBNull ? 0.0 : Convert.ToDouble(rdr["OrderQty"].ToString()),
                            DirectCost = rdr["DirectCost"] is DBNull ? 0.0 : Convert.ToDouble(rdr["DirectCost"].ToString()),
                            CommRate = rdr["CommRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["CommRate"].ToString()),
                            CommAmt = rdr["CommAmt"] is DBNull ? 0.0 : Convert.ToDouble(rdr["CommAmt"].ToString()),
                            OHRate = rdr["OHRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["OHRate"].ToString()),
                            OHAmt = rdr["OHAmt"] is DBNull ? 0.0 : Convert.ToDouble(rdr["OHAmt"].ToString()),
                            TaxRate = rdr["TaxRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxRate"].ToString()),
                            TaxAmt = rdr["TaxAmt"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TaxAmt"].ToString()),
                            TotalCost = rdr["TotalCost"] is DBNull ? 0.0 : Convert.ToDouble(rdr["TotalCost"].ToString()),
                            ProfitRate = rdr["ProfRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["ProfRate"].ToString()),
                            ProfitAmt = rdr["ProfAmt"] is DBNull ? 0.0 : Convert.ToDouble(rdr["ProfAmt"].ToString()),
                            SalePrice = rdr["SalePrice"] is DBNull ? 0.0 : Convert.ToDouble(rdr["SalePrice"].ToString()),
                            CostPP = rdr["CostPP"] is DBNull ? 0.0 : Convert.ToDouble(rdr["CostPP"].ToString()),
                            SalePP = rdr["SalePP"] is DBNull ? 0.0 : Convert.ToDouble(rdr["SalePP"].ToString()),
                            UsRate = rdr["USRate"] is DBNull ? 0.0 : Convert.ToDouble(rdr["USRate"].ToString()),
                            SaleUS = rdr["SaleUS"] is DBNull ? 0.0 : Convert.ToDouble(rdr["SaleUS"].ToString()),

                        };
                        CostSheetDtoList.Add(CostSheetData);
                    }

                }
            }
            return CostSheetDtoList;

        }

    }

}
