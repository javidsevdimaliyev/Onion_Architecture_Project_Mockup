using ZXing.Rendering;

namespace SolutionName.Application.Abstractions.Services
{
    public interface IQrCodeService
    {
        byte[] CreateQRCodeWithQrCoder(string data);
        public  SvgRenderer.SvgImage CreateQrCode(string data);

        public  byte[] CreateQrCodeImage(string data);
    }
}
