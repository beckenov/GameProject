using System;
using System.Collections.Generic;
using System.Linq;
using GnsGameProject.Data;
using GnsGameProject.Data.Models.Game;
using GnsGameProject.Service.Common.GameService;
using GnsGameProject.Service.Common.WordService;
using GnsGameProject.Service.Common.WordService.Models;

namespace GnsGameProject.Service.GameService
{
    public class GameService : IGameService
    {
        private readonly AppDbContext context;
        private readonly IWordService wordService;

        public GameService(
            AppDbContext context,
            IWordService wordService)
        {
            this.context = context;
            this.wordService = wordService;
        }

        public IEnumerable<WordModel> GetRandomWords()
        {
            var words = wordService.GetWords();
            return GenerateRandomWords(words);
        }

        public WordModel GetSecretWord(WordModel model)
        {
            return wordService.SetWordAsSecret(model);
        }

        public WordModel CheckWord(IEnumerable<WordModel> wordModels, string secretText, string letterOrWord, Guid userId)
        {
            var words = wordModels.ToList();
            if (words.Count > 0)
            {
                //слово которое нужно угадать
                var word = new WordModel { SecretWord = words[0].SecretWord, TryCount = words[0].TryCount, Question = words[0].Question, Id = words[0].Id};

                //Создается новый матч (игра)
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    WordId = word.Id,
                };

                word = wordService.CheckWord(word, secretText, letterOrWord);

                //если выиграл возвращает null
                if (word == null)
                {
                    //стирает угаданное слово из списка вопросов , что бы не повторялось 
                    words.RemoveAt(0);
                    match.Result = true;
                    context.Matches.Add(match);
                    context.SaveChanges();

                    if (words.Count > 0)
                    {
                        wordModels = words;
                        return GetSecretWord(new WordModel{Question = wordModels.First().Question,SecretWord = wordModels.First().SecretWord, TryCount = wordModels.First().TryCount});
                    }

                    return null;
                }
                else
                {
                    if (word.IsLose)
                    {
                        //lose
                        match.Result = false;
                        context.Matches.Add(match);
                        context.SaveChanges();
                        throw new ArgumentOutOfRangeException();
                    }
                    else
                    {
                        words[0].TryCount = word.TryCount;
                        return word;
                    }
                }
            }

            return null;
        }

        private IEnumerable<WordModel> GenerateRandomWords(IEnumerable<WordModel> words)
        {
            var wordList = new List<WordModel>(words);
            int count = wordList.Count;
            var stack = new Stack<WordModel>();
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var element = wordList[random.Next(0, wordList.Count)];
                stack.Push(element);
                wordList.Remove(element);
            }

            return stack;
        }

        
    }
}
