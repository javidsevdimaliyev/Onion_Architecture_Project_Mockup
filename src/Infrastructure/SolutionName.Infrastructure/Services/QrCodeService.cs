using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Utilities.Utility;
using ZXing.Rendering;

namespace SolutionName.Infrastructure.Services
{
    internal class QrCodeService : IQrCodeService
    {      
        public SvgRenderer.SvgImage CreateQrCode(string data)
        {
            return BarcodeQrcodeGenerator.CreateQrCode(data);
        }

        public byte[] CreateQrCodeImage(string data)
        {
            return BarcodeQrcodeGenerator.CreateQrCodeImage(data);
        }

        public byte[] CreateQRCodeWithQrCoder(string data)
        {
            return BarcodeQrcodeGenerator.CreateQRCodeWithQrCoder(data);
        }
    }
}
