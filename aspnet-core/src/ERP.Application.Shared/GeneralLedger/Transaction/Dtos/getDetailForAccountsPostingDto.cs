using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class getDetailForAccountsPostingDto
    {
        public List<getUsersForAccountPostingDto> getUsersForAccountPostingDto { get; set; }
        public List<getBooksForAccountPostingDto> getBooksForAccountPostingDto { get; set; }
        public getDetailForAccountsPostingDto()
        {
            getBooksForAccountPostingDto = new List<getBooksForAccountPostingDto>();
            getUsersForAccountPostingDto = new List<getUsersForAccountPostingDto>();
        }
    }
}
