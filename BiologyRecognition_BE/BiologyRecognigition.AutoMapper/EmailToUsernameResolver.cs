using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class EmailToUsernameResolver : IValueResolver<CreateAccountDTO, UserAccount, string>
    {
        public string Resolve(CreateAccountDTO source, UserAccount destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Email) && source.Email.Contains("@"))
                return source.Email.Split('@')[0];

            return "defaultuser";
        }
    }
}
