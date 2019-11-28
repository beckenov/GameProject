using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnsGameProject.Common;
using GnsGameProject.Data;
using GnsGameProject.Data.Models.Game;
using GnsGameProject.Data.Models.Users;
using GnsGameProject.Service.Common;
using GnsGameProject.Service.Common.UserService.Models;
using GnsGameProject.Service.Common.WordService;
using GnsGameProject.Service.Common.WordService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GnsGameProject.Service.WordService
{
    public class WordService : IWordService
    {
        private readonly AppDbContext context;

        public WordService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<WordModel> GetWord()
        {
            var word = await context.Words.FirstOrDefaultAsync();
            if (word != null)
            {
                return new WordModel()
                {
                    Id = word.Id,
                    SecretWord = word.SecretWord,
                    Question = word.Question,
                    TryCount = word.SecretWord.Length - 2
                };
            }

            return null;
        }

        public IEnumerable<WordModel> GetWords()
        {
            return from word in context.Words
                select new WordModel()
                {
                    Id = word.Id,
                    Question = word.Question,
                    SecretWord= word.SecretWord,
                    TryCount = word.SecretWord.Length - 2
                };
        }

        public async Task RemoveAsync(Guid id)
        {
            var word = await context.Words.FindAsync(id);
            if (word != null)
            {
                context.Words.Remove(word);
                await context.SaveChangesAsync();
            }
        }

        public async Task<OperationResult> Create(WordModel model)
        {
            var word = await context.Words.FirstOrDefaultAsync(x => x.SecretWord == model.SecretWord);
            var result = new OperationResult();
            if (word == null)
            {
                word = new Word
                {
                    Id = Guid.NewGuid(),
                    Question = model.Question,
                    SecretWord = model.SecretWord
                };

                context.Add(word);
                context.SaveChanges();
            }
            else
            {
                result.Message = "error";
            }

            return result;
        }
        public WordModel SetWordAsSecret(WordModel wordModel)
        {
            var secretWord = string.Join("", Enumerable.Repeat('*', wordModel.SecretWord.Length).ToArray());
            var sb = new StringBuilder(secretWord);
            secretWord = sb.ToString();
            wordModel.SecretWord = secretWord;
            return wordModel;
        }

        public WordModel CheckWord(WordModel wordModel, string secretText, string wordOrLetter)
        {
            if (wordOrLetter.Length > 1)
            {
                //new Match into Database
                if (string.Equals(wordModel.SecretWord, wordOrLetter, StringComparison.CurrentCultureIgnoreCase))
                {
                    //MatchResult = true
                    return null;
                }
                else
                {
                    //MatchResult = false
                    wordModel.IsLose = true;
                    return wordModel;
                }
            }
            else
            {
                var targetLetter = wordOrLetter.ToUpper()[0];
                var secretWordSymbols = wordModel.SecretWord.ToUpper().ToCharArray(0, wordModel.SecretWord.Length);
                var result = new StringBuilder(secretText);
                var isFoundLetter = false;
                for (var i = 0; i != secretWordSymbols.Length; ++i)
                {
                    if (secretWordSymbols[i] == targetLetter)
                    {
                        result[i] = targetLetter;
                        isFoundLetter = true;
                    }
                }

                if (wordModel.SecretWord.ToUpper().Equals(result.ToString()))
                {
                    return null;
                }

                wordModel.SecretWord = result.ToString();

                if (!isFoundLetter)
                {
                    wordModel.TryCount--;
                    if (wordModel.TryCount == 0)
                    {
                        //MatchResult = false
                        wordModel.IsLose = true;
                        return wordModel;
                    }
                    else
                    {
                        return wordModel;
                    }
                }

                return wordModel;
            }
        }

        public async Task PrepeareWordForEditAsync(WordModel model)
        {
            var word = await context.Words.FirstAsync(x => x.Id == model.Id);

            word.SecretWord = model.SecretWord;
            word.Question = model.Question;

            context.Words.Update(word);
            await context.SaveChangesAsync();
        }

        public async Task<WordModel> PrepeareWordForEditView(Guid id)
        {
            var word = (from w in context.Words
                where w.Id == id
                select new WordModel
                {
                    Id = w.Id,
                    Question = w.Question,
                    SecretWord = w.SecretWord
                }).FirstOrDefault();
            return word;
        }
    }
}
