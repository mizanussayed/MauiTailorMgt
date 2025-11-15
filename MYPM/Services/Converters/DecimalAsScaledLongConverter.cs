//using System;
//using System.Globalization;
//using Google.Cloud.Firestore;

//namespace MYPM.Services.Converters;

//public sealed class DecimalAsScaledLongConverter : IFirestoreConverter<decimal>
//{
//    public const int Scale = 2;
//    private const decimal Factor = 100m;

//    public object ToFirestore(decimal value)
//        => checked((long)Math.Round(value * Factor, 0, MidpointRounding.AwayFromZero));

//    public decimal FromFirestore(object value) => value switch
//    {
//        long l => l / Factor,
//        int i => i / Factor,
//        double d => Math.Round((decimal)d, Scale, MidpointRounding.AwayFromZero),
//        string s when decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var dv)
//            => Math.Round(dv, Scale, MidpointRounding.AwayFromZero),
//        null => 0m,
//        _ => throw new ArgumentException($"Unsupported Firestore value for decimal: {value.GetType().FullName}")
//    };
//}
