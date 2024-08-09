using System;
using System.IO;
using System.Media;

namespace Live2DCSharpSDK.Avalonia.Avatar;

public class QnaAudioManager()
{
    // SoundPlayer instance
    static readonly SoundPlayer player = new();

    // Qna List
    public static readonly QnaObj[] QnaList = [
            new()
            {
                Id = 0,
                Question = "Intro",
                Answer = "你好！請問有乜嘢可以幫到你？",
                AudioPath = "0.wav"
            },
            new()
            {
                Id = 1,
                Question = "海洋公園有咩園區？",
                Answer = "海洋公園有兩個主要樂園，第一個係「海濱樂園」，入面有「威威天地」、「亞洲動物天地」同「夢幻水都」；第二個係「高峰樂園」，入面有「滑浪飛船」、「熱帶激流」、「動感快車」、「翻天覆地」同「極速之旅 ── VR太空探索」。你可以選擇搭纜車或者海洋列車上去，沿途可以欣賞到壯觀嘅山海美景。",
                AudioPath = "1.wav"
            },
            new()
            {
                Id = 2,
                Question = "海洋公園有啲咩玩？",
                Answer = "海洋公園有好多嘢玩㗎。小朋友可以喺「威威天地」玩「彈彈屋」、「幻彩旋轉馬」。到咗「高峰樂園」，你可以玩「滑浪飛船」、「動感快車」，仲有「極速之旅 ── VR太空探索」。鍾意動物嘅可以去「亞洲動物天地」睇大熊貓、「尋鯊探秘」睇鯊魚，仲有「冰極天地」睇企鵝。園內有互動體驗，可以近距離接觸動物，了解佢哋嘅日常生活同保護野生動物嘅知識。海洋公園有好多嘢等緊你慢慢發掘！",
                AudioPath = "2.wav"
            },
            new()
            {
                Id = 3,
                Question = "海洋公園有啲咩食？",
                Answer = "喺海洋公園，你可以搵到好多唔同嘅食肆選擇。例如，喺「夢幻水都」區，有「海龍王餐廳」同「爐炭燒」兩間特色餐廳。 另外，如果你想嘆啲輕鬆嘅小食，「香港老大街」嘅「歡樂小食」同「動感天地」嘅「動感美食坊」都係好選擇。",
                AudioPath = "3.wav"
            },
            new()
            {
                Id = 4,
                Question = "海洋公園有啲咩動物？",
                Answer = "海洋公園有好多種動物，例如「澳洲歷奇」有無尾熊、「亞洲動物天地」有大熊貓，同「冰極天地」有企鵝，全部都好可愛！園內有互動體驗，可以近距離接觸動物，了解佢哋嘅生活同保護野生動物嘅知識。你可以喺「約會海象」摸海象、餵食；喺「豚聚一刻」同海豚玩水，甚至成為一小時嘅名譽大熊貓護理員。參加「神秘深海之夜」，可以喺「海洋奇觀」內露營，徹夜觀賞超過5000條魚，可以同鯆魚、鎚頭鯊相伴。",
                AudioPath = "4.wav"
            }
        ];

    /// <summary>
    /// Get the audio path for a given ID.
    /// </summary>
    /// <param name="id">The ID of the audio.</param>
    /// <returns>The full audio path.</returns>
    public static string GetAudioPath(int id)
    {
        string relativePath = @"\Resource";
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        string audioPath = Path.Combine(fullPath, QnaList[id].AudioPath);
        return audioPath;
    }

    /// <summary>
    /// Play the audio for a given ID.
    /// </summary>
    /// <param name="id">The ID of the audio.</param>
    public static void PlayAudio(int id)
    {
        try
        {
            string path = GetAudioPath(id);
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }
    }

    /// <summary>
    /// Play the audio from the specified path.
    /// </summary>
    /// <param name="path">The path of the audio file.</param>
    public static void PlayAudio(string path)
    {
        try
        {
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }
    }

    /// <summary>
    /// Stop the currently playing audio.
    /// </summary>
    public static void StopAudio()
    {
        try
        {
            player.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping audio: {ex.Message}");
        }
    }
}
