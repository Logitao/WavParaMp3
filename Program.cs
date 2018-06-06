using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WavParaMp3
{

    //  Wav para Mp3
    //  Antes de tudo instale o pacote nuGet NAudio.Lame ou digite no nuget CMD Install-Package NAudio.Lame
    //  Feito por: Logitao
    class Program
    {
        //Main
        static void Main(string[] args)
        {
            //Invoca a versão do mal do método main
            MainAsync(args).GetAwaiter().GetResult();
        }

        //Main asíncrono para não travar na hora de converter
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Convertendo");


            //Converte o arquivo wav em bytes, só informar o caminho completo
            byte[] bytesWav = FileToByteArray("justified.wav");

            Console.WriteLine("Espera");

            //Converte os bytes do WAV em bytes de MP3
            byte[] mp3Byte = await ConvertWavToMp3(bytesWav);


            //Converte os bytes MP3 em arquivo MP3
            ByteArrayToFile("teste66.mp3", mp3Byte);

            //foi
            Console.WriteLine("Foi");
            Console.ReadKey();
        }



        //Mesma coisa do <internal static byte[] ConvertWavToMp3I(byte[] wavFile)>
        //Unica diferença: ele chama o outro só que com async e await para ir mais rápido
        public static async Task<byte[]> ConvertWavToMp3(byte[] wavFile)
        {
            //Invoca aquele método la
            var task = Task.Run(() => ConvertWavToMp3I(wavFile));
            var output = await task;

            //Retorna a conversão
            return output;
        }

        //Converte um arquivo em byte[]
        public static byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        //Converte uma byte[] em um arquivo
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }


        //Converte bytes do arquivo WAV para arquivo MP3
        //Este método está declarado internal por causa que é ultilizado numa versão async para não travar
        //Este método pode ser chamado diretamente, mas a performance não será a mesma
        
        internal static byte[] ConvertWavToMp3I(byte[] wavFile)
        {
           
            using (var retMs = new MemoryStream())
            using (var ms = new MemoryStream(wavFile))
            using (var rdr = new WaveFileReader(ms))

            using (var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, 128))
            {
                rdr.CopyTo(wtr);
                return retMs.ToArray();
            }

        }
    }
}
