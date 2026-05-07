import { useMemo, useState } from 'react';
import { useInventory } from '../hooks/useInventory';
import { useCategories } from '../hooks/useCategories';
import { useBrands } from '../hooks/useBrands';
import ItemTable from '../components/inventory/ItemTable';
import CategoryFilter from '../components/inventory/CategoryFilter';
import ItemFormModal from '../components/inventory/ItemFormModal';

export default function InventoryPage() {
  const { items, loading, error, createItem } = useInventory();
  const { categories, createCategory } = useCategories();
  const { brands, createBrand } = useBrands();

  const [categoryFilter, setCategoryFilter] = useState('all');
  const [sort, setSort] = useState({ key: 'name', dir: 'asc' });
  const [showAdd, setShowAdd] = useState(false);

  const categoryMap = useMemo(
    () => new Map(categories.map((c) => [c.categoryId, c.name])),
    [categories]
  );
  const brandMap = useMemo(
    () => new Map(brands.map((b) => [b.brandId, b.name])),
    [brands]
  );

  const visibleItems = useMemo(() => {
    const base =
      categoryFilter === 'all'
        ? items
        : items.filter((i) => i.categoryId === Number(categoryFilter));

    const enriched = base.map((i) => ({
      ...i,
      categoryName: categoryMap.get(i.categoryId) ?? '',
      brandName: i.brandId != null ? brandMap.get(i.brandId) ?? '' : '',
    }));

    const sorted = [...enriched].sort((a, b) => {
      const av = a[sort.key];
      const bv = b[sort.key];
      if (av == null && bv == null) return 0;
      if (av == null) return 1;
      if (bv == null) return -1;
      if (av < bv) return sort.dir === 'asc' ? -1 : 1;
      if (av > bv) return sort.dir === 'asc' ? 1 : -1;
      return 0;
    });

    return sorted;
  }, [items, categoryFilter, sort, categoryMap, brandMap]);

  const onSort = (key) =>
    setSort((s) => ({
      key,
      dir: s.key === key && s.dir === 'asc' ? 'desc' : 'asc',
    }));

  return (
    <div className="p-6 space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-semibold">Inventory</h1>
        <button
          onClick={() => setShowAdd(true)}
          className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-500"
        >
          Add Item
        </button>
      </div>

      <CategoryFilter
        categories={categories}
        value={categoryFilter}
        onChange={setCategoryFilter}
      />

      {error && <p className="text-red-600">{error}</p>}
      {loading ? (
        <p>Loading…</p>
      ) : (
        <ItemTable
          items={visibleItems}
          categoryMap={categoryMap}
          brandMap={brandMap}
          sort={sort}
          onSort={onSort}
        />
      )}

      {showAdd && (
        <ItemFormModal
          mode="add"
          categories={categories}
          brands={brands}
          onCreateCategory={createCategory}
          onCreateBrand={createBrand}
          onSubmit={async (payload) => {
            await createItem(payload);
            setShowAdd(false);
          }}
          onClose={() => setShowAdd(false)}
        />
      )}
    </div>
  );
}
