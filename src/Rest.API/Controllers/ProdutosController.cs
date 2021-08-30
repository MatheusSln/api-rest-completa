using API.Business.Intefaces;
using API.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest.API.Data.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.API.Controllers
{
    [Route("api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(INotificador notificador, 
                                  IProdutoRepository produtoRepository, 
                                  IProdutoService produtoService, 
                                  IMapper mapper) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoDto>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDto>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> ObterPorId(Guid id)
        {
            var produtoDto = await ObterProduto(id);

            if (produtoDto == null) return NotFound();

            return produtoDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Adicionar(ProdutoDto produtoDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoDto.Imagem;
            if (!UploadArquivo(produtoDto.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoDto);
            }

            produtoDto.Imagem = imagemNome;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoDto));

            return CustomResponse(produtoDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(produto);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        //private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
        //{
        //    if (arquivo == null || arquivo.Length == 0)
        //    {
        //        NotificarErro("Forneça uma imagem para este produto!");
        //        return false;
        //    }

        //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgPrefixo + arquivo.FileName);

        //    if (System.IO.File.Exists(path))
        //    {
        //        NotificarErro("Já existe um arquivo com este nome!");
        //        return false;
        //    }

        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await arquivo.CopyToAsync(stream);
        //    }

        //    return true;
        //}

        private async Task<ProdutoDto> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterProdutoFornecedor(id));
        }
    }
}
