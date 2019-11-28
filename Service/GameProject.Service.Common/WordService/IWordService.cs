using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GnsGameProject.Service.Common.WordService.Models;

namespace GnsGameProject.Service.Common.WordService
{
    public interface IWordService
    {
        Task<WordModel> GetWord();

        IEnumerable<WordModel> GetWords();

        Task RemoveAsync(Guid id);

        Task<OperationResult> Create(WordModel model);

        Task PrepeareWordForEditAsync(WordModel model);

        Task<WordModel> PrepeareWordForEditView(Guid id);

        WordModel SetWordAsSecret(WordModel wordModel);

        WordModel CheckWord(WordModel wordModel, string secretText, string wordOrLetter);
    }
}
