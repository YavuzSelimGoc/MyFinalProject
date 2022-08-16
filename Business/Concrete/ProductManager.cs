using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;


        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        [SecuredOperation("product.add,admin")]
        public IResult Add(Product product)
        {
            IResult result= BusinessRules.Run(ChechkIfProductCountOfCategoryCorret(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded());
            
            if (result!=null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccesResult(Messages.ProductAdded);
        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 36)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccesDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccesDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id), Messages.ProductsListed);
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccesDataResult<Product>(_productDal.Get(x => x.ProductId == productId), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetByUnitePrice(decimal min, decimal max)
        {
            return new SuccesDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice > min && p.UnitPrice < max), Messages.ProductsListed);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 16)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccesDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductsListed);
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            ChechkIfProductCountOfCategoryCorret(product.CategoryId);
            return new SuccesResult();
        }

        private IResult ChechkIfProductCountOfCategoryCorret(int categoryId)
            {
                var CategoryNumber = _productDal.GetAll(x => x.CategoryId == categoryId).Count;
                if (CategoryNumber > 10)
                {
                    return new ErrorResult(Messages.ProductCountOfCategoryError);
                }
                return new SuccesResult();
            }
        private IResult CheckIfProductNameExists(string productName)
        {
            var ProductName = _productDal.GetAll(x => x.ProductName == productName).Any();
            if (ProductName)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccesResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var categoryCount = _categoryService.GetAll();
            if (categoryCount.Data.Count>999)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccesResult();
        }
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
