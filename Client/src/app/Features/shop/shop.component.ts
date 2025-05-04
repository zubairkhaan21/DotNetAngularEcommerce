import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../Core/services/shop.service';
import { Product } from '../../Shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { ShopParams } from '../../Shared/models/shopParams';
import { Pagination } from '../../Shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
  ],

  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 20, 50];
  products?: Pagination<Product>;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  ngOnInit(): void {
    this.initilizeShop();
  }

  initilizeShop() {
    this.shopService.getTypes();
    this.shopService.getBrand();
    this.getProducts;
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => (this.products = response),
      error: (error) => console.log(error),
    });
  }

  onSearchChange(){
    this.shopParams.pageNumber = 1; // Reset to first page on search change
    this.getProducts();
  }


  handlePageChanged(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1; // Reset to first page on sort change
      this.getProducts();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedType: this.shopParams.types,
        selectedBrand: this.shopParams.brands,
      },
    });
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          console.log(result);
          this.shopParams.types = result.selectedType;
          this.shopParams.brands = result.selectedBrand;
          this.shopParams.pageNumber = 1; // Reset to first page on filter change
          this.getProducts();
        }
      },
    });
  }
}
