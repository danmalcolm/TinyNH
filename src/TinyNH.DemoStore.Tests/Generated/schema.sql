
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_Product_Supplier_SupplierId]') AND parent_object_id = OBJECT_ID('Product'))
alter table Product  drop constraint FK_Product_Supplier_SupplierId


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_Product_Category_CategoryId]') AND parent_object_id = OBJECT_ID('Product'))
alter table Product  drop constraint FK_Product_Category_CategoryId


    if exists (select * from dbo.sysobjects where id = object_id(N'Product') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Product

    if exists (select * from dbo.sysobjects where id = object_id(N'Supplier') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Supplier

    if exists (select * from dbo.sysobjects where id = object_id(N'Category') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Category

    create table Product (
        ProductId UNIQUEIDENTIFIER not null,
       Code NVARCHAR(20) not null unique,
       Name NVARCHAR(50) not null,
       Description NVARCHAR(1000) not null,
       UnitsInStock INT not null,
       SupplierId UNIQUEIDENTIFIER not null,
       CategoryId UNIQUEIDENTIFIER not null,
       primary key (ProductId)
    )

    create table Supplier (
        SupplierId UNIQUEIDENTIFIER not null,
       Code NVARCHAR(20) not null,
       Name NVARCHAR(50) not null,
       primary key (SupplierId)
    )

    create table Category (
        CategoryId UNIQUEIDENTIFIER not null,
       Code NVARCHAR(20) not null,
       Name NVARCHAR(50) not null,
       primary key (CategoryId)
    )

    alter table Product 
        add constraint FK_Product_Supplier_SupplierId 
        foreign key (SupplierId) 
        references Supplier

    alter table Product 
        add constraint FK_Product_Category_CategoryId 
        foreign key (CategoryId) 
        references Category
