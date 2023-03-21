using ImageSharp = SixLabors.ImageSharp;

namespace Tuif.Dom;

public class Image : Node 
{
    private Color[][]? _pixels;
    private uint _offsetX = 0, _offsetY = 0;

    private uint _imgWidth, _imgHeight;
    public Image(uint width, uint height) : base(width,height)
    {
    }

    public Image() : base(0,0)
     {}

    public override void UpdateSize(uint width, uint height)
    {
        base.UpdateSize(width, height);

        if (width > _imgWidth) 
        {
            _offsetX = (uint)(width - _imgWidth) / 2;
        }

        if (height > _imgHeight) 
        {
            _offsetY = (uint)(height - _imgHeight) / 2;
        }
    }

    public bool Load(string path) 
    {
        if (!File.Exists(path)) 
        {
            return false;
        }

        using var image = ImageSharp.Image.Load<ImageSharp.PixelFormats.Rgba32>(path);
        
        _pixels = new Color[image.Height][];

        for (int y = 0; y < image.Height; y++)
        {
            _pixels[y] = new Color[image.Width];
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image[x,y];
                if (pixel.A != 0) 
                {
                    _pixels[y][x] = new Color(pixel.R, pixel.G, pixel.B);
                }
                else 
                {
                    _pixels[y][x] = Color.DefaultBackground;
                }
            }
        }

        _imgWidth = (uint)image.Width * 2;
        _imgHeight = (uint)image.Height;

        if (_width > _imgWidth) 
        {
            _offsetX = (uint)(_width - _imgWidth) / 2;
        }

        if (_height > _imgHeight) 
        {
            _offsetY = (uint)(_height - _imgHeight) / 2;
        }

        return true;
    }

    public uint GetImgWidth() => _imgWidth / 2;
    public uint GetImgHeight() => _imgHeight;

    public override void Render(Buffer buff)
    {
        if (_pixels == null) 
        {
            return;
        }

        uint maxHeight = Math.Min(_height, _imgHeight);
        uint maxWidth = Math.Min(_width, _imgWidth);

        for (uint y = 0; y < maxHeight; y++)
        {
            for (uint x = 0; x < maxWidth; x++)
            {
                buff.Write('█', x + _posX + _offsetX, y + _posY + _offsetY, _pixels[y][x / 2]);
            }
        }
    }
}