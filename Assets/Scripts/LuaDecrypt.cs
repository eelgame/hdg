using System.IO;
using System.Text;
using UnityEngine;

public class LuaDecrypt : MonoBehaviour
{
    public static string privatekey =
        "<RSAKeyValue><Modulus>kehYgewbsgoobO6N459osAuKvTfleCkm0WWCssU1kaBlkIJtbLtsL9tCP73OyeIP7xOgO8ijF4OD38ipyBIFhgKkzGDnEo+v7GOQws8G1jgQd+NLLOcDnXOR+WXVmIS9cbXaJKEUubQnu198/FNV0N+SLBAYhNzSZRu3B7BDfe8=</Modulus><Exponent>EQ==</Exponent><P>+cAwsysQ4b2cMmRyK0rZE0DfzYdYhBA97sS+b+bzobYnrlHTRAbKfW77AuUBYkYiYIyLpFsv3pdSStcb70aSqw==</P><Q>lY75psNwfBHMRPhfouSFknHDAtlqQHeB007WVf+Hnz63BAqM7QLMLRtfM1YIrsbfjPl5WqxeeS6dvNVRM4ghzQ==</Q><DP>oZp53VgZ+3qwXNeVKxJQOaJysipmc5IJ9NmoZoZhaKMKnfi14LkZnHT8xaM9IXis1Q+lppVbNa01P3whIlrXXw==</DP><DQ>jMLM2TBp3i7eXwfhikB9twGoeyb6lwcQxuDJulnpDlkkuIJmhLdWwQqz9BS84LsswOrMkZMrvVkM7fXyEmIBsQ==</DQ><InverseQ>Fi4C8LDc03mJEMbo/qKtVlxoWTI517Ka+9hKX1kpLd8aqSpMjRTh/RljwA01GTqTz+RFusum0DWPOYgpfgbBhg==</InverseQ><D>XmkqNfMg+rtHVYtMwHY0riWWAfb95Fbc4dhFgrvXXjqcEjZGzeKvakKUR1yy+xnOEyrRF5/xDzb6+jaL+e2LGXfN/23sIbbckZjWUKYxESJ480h5j/gyB+tRyKO41rNjqmuru1eOvFGD85Mqq9CJdtjar93dzLBxeO7Zr0x4vpk=</D></RSAKeyValue>";

    // Token: 0x0400280C RID: 10252
    public static string publickey =
        "<RSAKeyValue><Modulus>kehYgewbsgoobO6N459osAuKvTfleCkm0WWCssU1kaBlkIJtbLtsL9tCP73OyeIP7xOgO8ijF4OD38ipyBIFhgKkzGDnEo+v7GOQws8G1jgQd+NLLOcDnXOR+WXVmIS9cbXaJKEUubQnu198/FNV0N+SLBAYhNzSZRu3B7BDfe8=</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>";

    // Use this for initialization
    private void Start()
    {
        var files = Directory.GetFiles(Application.dataPath + "/XLuaScripts", "*.lua", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var result = Encoding.UTF8.GetString(ReadLua(publickey,
                File.ReadAllBytes(file)));

            File.WriteAllText(file + ".decrypt", result);
        }
    }

    public static byte[] DeXorCalc(byte[] xordata, byte[] xorkey)
    {
        var num = xordata.Length - 1 - 15;
        var array = new byte[xordata.Length];
        var array2 = new byte[16];
        for (var i = 0; i < 16; i++) array2[i] = xorkey[i];
        while (num != 0)
        {
            for (var j = 0; j < 16; j++) array[num + j] = (byte) (xordata[num + j] ^ xordata[num - 16 + j]);
            num -= 16;
        }

        for (var k = 0; k < 16; k++) array[k] = (byte) (xordata[k] ^ array2[k]);
        return array;
    }

    public static byte[] ReadLua(string publickey, byte[] data)
    {
        var array = new byte[128];
        var array2 = new byte[16];
        var array3 = new byte[data.Length - 128 - 16];
        for (var i = 0; i < array.Length; i++) array[i] = data[i];
        for (var j = 0; j < array2.Length; j++) array2[j] = data[array.Length + j];
        for (var k = 0; k < array3.Length; k++) array3[k] = data[array.Length + array2.Length + k];
        var array4 = DeXorCalc(array3, array);
        var num = array4.Length - 1;
        while (array4[num] == 0) num--;
        var num2 = num + 1;
        var array5 = new byte[num2];
        for (var l = 0; l < num2; l++) array5[l] = array4[l];
        return array5;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}