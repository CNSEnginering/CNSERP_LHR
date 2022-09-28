using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.AccountPayables.Dtos;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.AccountPayables
{
    public class APTransactionListAppService : ERPAppServiceBase
    {
        private readonly IRepository<GLBOOKS> _bookrepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;

        public APTransactionListAppService(IRepository<GLBOOKS> accountsPostingRepository, IRepository<User, long> userRepository, IRepository<GLTRHeader> gltrHeaderRepository)
        {
            _bookrepository = accountsPostingRepository;
            _userRepository = userRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
        }

      
        public async Task<ListResultDto<GetBookViewModeldto>> GetBookList()
        {
            var query = _bookrepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Select(i => new { i.BookName, i.BookID }).Distinct();
            var groupCategoryList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<GetBookViewModeldto>();
            //lookupTableDtoList.Add(new GetBookViewModeldto
            //{

            //    BookId = "All",
            //    BookName = "All"
            //});
            foreach (var groupCategory in groupCategoryList)
            {
                lookupTableDtoList.Add(new GetBookViewModeldto
                {

                    BookId = groupCategory.BookID,
                    BookName = groupCategory.BookName
                });
            }

            return new ListResultDto<GetBookViewModeldto>(
                lookupTableDtoList
            );
        }
        public Task GetReportParm(APTransactionListViewDto input)
        {
            return null;
        }

        //public async Task<PostApTransactionParm>(APTransactionListViewDto input)
        //{

        //    var user = input;
        //    //await user;

        //}

        public async Task<ListResultDto<MonthListDto>> GetMonthList(string bookid)
        {

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

            var q = from o in _gltrHeaderRepository.GetAll()

                    where o.BookID == bookid
                    select new
                    {
                        MonthId = o.DocMonth,
                        MonthName = "",
                    };
            var monthlist = await q.Distinct()
               .ToListAsync();
            var monthobj = new List<MonthListDto>();
            foreach (var item in monthlist)
            {
                monthobj.Add(new MonthListDto
                {

                    MonthId = Convert.ToInt32(item.MonthId),
                    MonthName = mfi.GetMonthName(item.MonthId).ToString()
                });
            }

            return new ListResultDto<MonthListDto>(
                monthobj
            );
        }

        public async Task<ListResultDto<YearListDto>> GetYearList(string bookid)
        {
            var q = from o in _gltrHeaderRepository.GetAll()
                    where o.BookID == bookid
                    select new
                    {
                        MonthId = o.DocDate.Year,

                    };
            var monthlist = await q.Distinct()
               .ToListAsync();
            var yearslistdto = new List<YearListDto>();
            foreach (var item in monthlist)
            {
                yearslistdto.Add(new YearListDto
                {
                    Years = Convert.ToInt32(item.MonthId),

                });
            }
            return new ListResultDto<YearListDto>(
                yearslistdto
            );
        }
        public async Task<ListResultDto<UserDto>> GetUserList()
        {
            var query = _userRepository.GetAll().Select(o => o.UserName).Distinct();
            var groupCategoryList = await query.ToListAsync();

            var userTableDto = new List<UserDto>();
            userTableDto.Add(new UserDto
            {
                Username = "All"
            });
            foreach (var groupCategory in groupCategoryList)
            {
                userTableDto.Add(new UserDto
                {
                    Username = groupCategory.ToString()
                });
            }

            return new ListResultDto<UserDto>(
                userTableDto
            );
        }

    }

    }
