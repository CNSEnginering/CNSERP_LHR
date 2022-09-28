using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ERP.SupplyChain.Inventory
{
    public class GetDataAppService : ERPAppServiceBase
    {
        private List<GetDataViewDto> dataList;

        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<CurrencyRate, string> _currencyRateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        public GetDataAppService(IRepository<ICLocation> icLocationRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IRepository<CurrencyRate, string> currencyRateRepository, IRepository<TransactionType> transactionType, IRepository<GLBOOKS> glbooksRepository)
        {
            _icLocationRepository = icLocationRepository;
            _userRepository = userRepository;
            _glbooksRepository = glbooksRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _currencyRateRepository = currencyRateRepository;
            _transactionTypeRepository = transactionType;
        }

        public List<GetDataViewDto> GetList(string target)
        {
            
            switch (target)
            {
                case "ICLocations":
                    dataList = GetUserICLocation();
                    break;
                case "PICLocations":
                    dataList = GetUserPICLocation();
                    break;
                case "ICLocationsForAdmin":
                    dataList = GetICLocation();
                    break;
                case "Users":
                    dataList = GetUsers();
                    break;
                case "GLBooks":
                    dataList = GetGLBooks();
                    break;
                case "SaleType":
                    dataList = GetGLSaleType();
                    break;
                case "Currency":
                    dataList = GetCurrency();
                    break;
                default:
                    break;
            }
            return dataList;
        }
        private List<GetDataViewDto> GetUsers()
        {
            var Users = UserManager.Users;
            var UsersList = from o in Users
                            select new GetDataViewDto
                            {
                                Id = (int)o.Id,
                                DisplayName = o.UserName
                            };
            return UsersList.ToList();
        }
        private List<GetDataViewDto> GetCurrency()
        {
            var query = _currencyRateRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new GetDataViewDto
                                 {
                                     DescId = o.Id,
                                     DisplayName = o.CURNAME

                                 };

            var getData = lookupTableDto.ToList();
            return getData;
        }
        private List<GetDataViewDto> GetICLocation()
        {
            var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var locationsList = from o in locations
                                select new GetDataViewDto
                            {
                                Id = o.LocID,
                                DisplayName =o.LocID +" "+ o.LocName
                            };
            return locationsList.ToList();
        }
        private List<GetDataViewDto> GetUserPICLocation()
        {
            IQueryable<GetDataViewDto> lookupTableDto;
            var userid = userInfo();
            if (userid.ToLower() != "admin")
            {
                //var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == userid && c.Status == true);
                // .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                // e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e..ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);
                var ParentLoc = _icLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.ParentID!=0).Select(c=>c.ParentID).ToList();
                var locQuery = _icLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && ParentLoc.Contains(c.LocID));

                 lookupTableDto = from p in locQuery
                                  //join p in locQuery on o.LocId equals p.LocID
                                     select new GetDataViewDto
                                     {
                                         Id = Convert.ToInt32(p.LocID),
                                         DisplayName = p.LocName
                                     };

            }
            else
            {
                var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                 lookupTableDto = from o in locations
                                    select new GetDataViewDto
                                    {
                                        Id = o.LocID,
                                        DisplayName = o.LocID + " " + o.LocName
                                    };
            }


            return lookupTableDto.ToList();
        }

        private List<GetDataViewDto> GetUserICLocation()
        {
            IQueryable<GetDataViewDto> lookupTableDto;
            var userid = userInfo();
            if (userid.ToLower() != "admin")
            {
                var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == userid && c.Status == true);
                // .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                // e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e..ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);
                var locQuery = _icLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId);

                lookupTableDto = from o in query
                                 join p in locQuery on o.LocId equals p.LocID
                                 select new GetDataViewDto
                                 {
                                     Id = Convert.ToInt32(o.LocId),
                                     DisplayName = p.LocName
                                 };

            }
            else
            {
                var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                lookupTableDto = from o in locations
                                 select new GetDataViewDto
                                 {
                                     Id = o.LocID,
                                     DisplayName = o.LocID + " " + o.LocName
                                 };
            }


            return lookupTableDto.ToList();
        }
        public string userInfo()
        {
            var data = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).FirstOrDefault();
            return data.Name;
        }
       

        private List<GetDataViewDto> GetGLBooks()
        {
            var books = _glbooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID != "JV" && o.Integrated==true && o.INACTIVE==false);
            var booksList = (from o in books
                            select new GetDataViewDto
                            {
                                DescId = o.BookID,
                                DisplayName = o.BookName
                            });
            return booksList.ToList();
        }
        private List<GetDataViewDto> GetGLSaleType()
        {
            var books = _transactionTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Active==true);
            var booksList = (from o in books
                             select new GetDataViewDto
                             {
                                 DescId = o.TypeId,
                                 DisplayName = o.Description
                             });
            return booksList.ToList();
        }

    }

}
