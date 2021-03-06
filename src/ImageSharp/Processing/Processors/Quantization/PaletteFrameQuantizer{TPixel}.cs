// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Processing.Processors.Quantization
{
    /// <summary>
    /// Encapsulates methods to create a quantized image based upon the given palette.
    /// <see href="http://msdn.microsoft.com/en-us/library/aa479306.aspx"/>
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal struct PaletteFrameQuantizer<TPixel> : IFrameQuantizer<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        private readonly ReadOnlyMemory<TPixel> palette;
        private readonly EuclideanPixelMap<TPixel> pixelMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteFrameQuantizer{TPixel}"/> struct.
        /// </summary>
        /// <param name="configuration">The configuration which allows altering default behaviour or extending the library.</param>
        /// <param name="options">The quantizer options defining quantization rules.</param>
        /// <param name="colors">A <see cref="ReadOnlyMemory{TPixel}"/> containing all colors in the palette.</param>
        [MethodImpl(InliningOptions.ShortMethod)]
        public PaletteFrameQuantizer(Configuration configuration, QuantizerOptions options, ReadOnlyMemory<TPixel> colors)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(options, nameof(options));

            this.Configuration = configuration;
            this.Options = options;

            this.palette = colors;
            this.pixelMap = new EuclideanPixelMap<TPixel>(colors);
        }

        /// <inheritdoc/>
        public Configuration Configuration { get; }

        /// <inheritdoc/>
        public QuantizerOptions Options { get; }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public QuantizedFrame<TPixel> QuantizeFrame(ImageFrame<TPixel> source, Rectangle bounds)
            => FrameQuantizerExtensions.QuantizeFrame(ref this, source, bounds);

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public ReadOnlyMemory<TPixel> BuildPalette(ImageFrame<TPixel> source, Rectangle bounds)
            => this.palette;

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public byte GetQuantizedColor(TPixel color, ReadOnlySpan<TPixel> palette, out TPixel match)
            => (byte)this.pixelMap.GetClosestColor(color, out match);

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
