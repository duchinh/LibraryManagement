export interface CategoryDto {
    id: string;
    name: string;
    description: string;
  }
  
  export interface CreateCategoryDto {
    name: string;
    description?: string;
  }
  
  export interface UpdateCategoryDto extends CreateCategoryDto {
    id: string;
  }