using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.UiEditor.Data
{
    public sealed class InterfaceSize
    {
        public static readonly InterfaceSize VerySmall = new InterfaceSize(Properties.Language.InterfaceSize_VerySmall, 2.12677);
        public static readonly InterfaceSize Small = new InterfaceSize(Properties.Language.InterfaceSize_Small, 1.81818);
        public static readonly InterfaceSize Normal = new InterfaceSize(Properties.Language.InterfaceSize_Normal, 1.42857);
        public static readonly InterfaceSize Large = new InterfaceSize(Properties.Language.InterfaceSize_Large, 1.17647);
        public static readonly InterfaceSize VeryLarge = new InterfaceSize(Properties.Language.InterfaceSize_VeryLarge, 1);

        public static IEnumerable<InterfaceSize> InterfaceSizes => new InterfaceSize[]
        {
            VerySmall,
            Small,
            Normal,
            Large,
            VeryLarge
        };

        public const double SizeExDefaultCoef = 1.7678589107160535731964303392875;
        private InterfaceSize(string title, double scaleFactor)
        {
            this.ScaleFactor = scaleFactor;
            this.SizeExBase = SizeExDefaultCoef * scaleFactor;
            this.Title = title;
        }

        public string Title { get; }
        public double ScaleFactor { get; }
        public double SizeExBase { get; }

        public double FontSizeToSizeEx(double point, double resolutionHeight) => this.SizeExBase / resolutionHeight * point;
        public double FontSizeToSizeEx(double point, Rect viewPort) => this.SizeExBase / viewPort.Bottom * point;

        public double SizeExToFontSize(double sizeEx, double resolutionHeight) => sizeEx / this.SizeExBase * resolutionHeight;
        public double SizeExToFontSize(double sizeEx, Rect viewPort) => sizeEx / this.SizeExBase * viewPort.Bottom;
    }
}
