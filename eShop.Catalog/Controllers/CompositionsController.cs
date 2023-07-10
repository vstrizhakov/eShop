using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Compositions;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/compositions")]
    [ApiController]
    [Authorize]
    public class CompositionsController : ControllerBase
    {
        private readonly ICompositionRepository _repository;
        private readonly IMapper _mapper;

        public CompositionsController(ICompositionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Composition>>> GetCompositions()
        {
            var ownerId = User.GetAccountId();
            var compositions = await _repository.GetCompositionsAsync(ownerId.Value);
            return Ok(compositions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Composition>> GetComposition([FromRoute] Guid id)
        {
            var composition = await _repository.GetCompositionByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            return composition;
        }

        [HttpPost]
        public async Task<ActionResult<Composition>> PostComposition(
            [FromForm] CreateCompositionRequest request,
            [FromServices] IFileManager fileManager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var composition = _mapper.Map<Composition>(request);
            var ownerId = User.GetAccountId();
            composition.OwnerId = ownerId.Value;

            var image = request.Image;
            using var imageStream = image.OpenReadStream();
            var imagePath = await fileManager.SaveAsync(Path.Combine("Compositions", composition.Id.ToString(), "Images"), Path.GetExtension(image.FileName), imageStream);

            // TODO: Handle products` images (request.Products.Images)

            composition.Images.Add(new CompositionImage
            {
                Path = imagePath,
            });

            await _repository.CreateCompositionAsync(composition);

            return CreatedAtAction("GetComposition", new { id = composition.Id }, composition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposition(
            [FromRoute] Guid id,
            [FromServices] IFileManager fileManager)
        {
            var composition = await _repository.GetCompositionByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            foreach (var image in composition.Images)
            {
                await fileManager.DeleteAsync(image.Path);
            }

            await _repository.DeleteCompositionAsync(composition);

            return NoContent();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Broadcast(
        //    [FromRoute] string id,
        //    [FromServices] ITelegramBotClient telegramBotClient,
        //    [FromServices] IViberBotClient viberBotClient,
        //    [FromServices] IPublicUriBuilder publicUriBuilder,
        //    [FromServices] ITelegramContextConverter telegramContextConverter)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userId = User.GetSub();
        //    var composition = await _context.Compositions
        //        .Include(e => e.Images)
        //        .Include(e => e.Products)
        //        .Where(e => e.OwnerId == userId)
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //    if (composition == null)
        //    {
        //        return NotFound();
        //    }

        //    var clients = await _context.Users
        //        .Include(e => e.TelegramChats)
        //            .ThenInclude(e => e.TelegramChat)
        //        .Include(e => e.ViberUser)
        //        .Include(e => e.ViberChatSettings)
        //        .Where(e => e.ProviderId == userId)
        //        .ToListAsync();

        //    var image = composition.Images.FirstOrDefault();
        //    if (image != null)
        //    {
        //        var imageLink = publicUriBuilder.Path(image.Path);
        //        var text = string.Join("\n\n", composition.Products.Select(e => $"{e.Name} - {e.Prices.LastOrDefault()}"));

        //        foreach (var client in clients)
        //        {
        //            var telegramChats = client.TelegramChats
        //                .Where(e => e.IsEnabled)
        //                .Select(e => e.TelegramChat);
        //            foreach (var telegramChat in telegramChats)
        //            {
        //                var media = new InputMediaPhoto(new InputFileUrl(imageLink));
        //                media.Caption = text;
        //                await telegramBotClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), new List<IAlbumInputMedia>() { media });
        //            }

        //            var viberChat = client.ViberChatSettings;
        //            if (viberChat != null && viberChat.IsEnabled)
        //            {
        //                var viberUser = client.ViberUser;

        //                var sender = new ViberBot.User
        //                {
        //                    Name = "Test",
        //                };
        //                var keyboard = new Keyboard
        //                {
        //                    Buttons = new[]
        //                    {
        //                        new Button
        //                        {
        //                            Rows = 1,
        //                            Text = "Налаштування анонсів",
        //                            ActionBody = telegramContextConverter.Serialize(ViberContext.Settings),
        //                        },
        //                    },
        //                };
        //                await viberBotClient.SendPictureMessageAsync(viberUser.ExternalId, sender, imageLink, text, keyboard: keyboard);
        //            }
        //        }
        //    }

        //    return RedirectToAction(nameof(Details), new { id });
        //}
    }
}
