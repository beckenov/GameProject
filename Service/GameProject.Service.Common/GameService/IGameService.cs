using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GnsGameProject.Service.Common.WordService.Models;

namespace GnsGameProject.Service.Common.GameService
{
    public interface IGameService
    {
        //Получает рандом слова из БД
        IEnumerable<WordModel> GetRandomWords();

        //Пребразует слова в звездочки
        WordModel GetSecretWord(WordModel model);

        //Проверяет слово на наличие букв или слова 
        WordModel CheckWord(IEnumerable<WordModel> words, string secretText, string letterOrWord, Guid userId);
    }
}
