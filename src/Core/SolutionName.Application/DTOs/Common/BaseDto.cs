using SolutionName.Application.Utilities.Utility;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SolutionName.Application.DTOs.Common
{
    [DataContract]
    public class BaseDto<TKey> : BaseDto
    {
        public string IdHash { get; set; }

        [JsonIgnore]
        [DataMember(EmitDefaultValue = false)]
        public TKey Id { get { return Decrypt<TKey>(IdHash); } set { IdHash = Encrypt(value); } }
       
        

    }

    public class BaseDto
    {
        #region EncryptDecrypt
        protected string? Encrypt(object? id)
        {
            if (id is null)
                return null;

            return TextEncryption.Encrypt(id!.ToString());
        }

        protected T? Decrypt<T>(string id)
        {
            return TextEncryption.Decrypt<T>(id);
        }

        protected int[] Decrypt(string[] idsHash)
        {
            return TextEncryption.Decrypt(idsHash);
        }

        protected string[] Encrypt(int[] ids)
        {

            if (ids == null)
                return null;

            string[] idsHash = new string[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                idsHash[i] = TextEncryption.Encrypt(ids[i].ToString());
            }

            return idsHash;
        }
        #endregion
    }
}
