using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Models.FileManagement
{
    public class FileSaveRequest
    {

        public string FileName { get; set; }

        public byte[] Content { get; set; }

        /// <summary>
        /// File base64 content
        /// </summary>
        public string Base64Content { get; set; }
    }
}
