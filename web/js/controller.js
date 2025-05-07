const API_BASE_URL = 'http://localhost:5006/api/Products';

// DOM Elements
const productForm = document.getElementById('productForm');
const productTableBody = document.getElementById('productTableBody');
const searchInput = document.getElementById('searchInput');
const cancelEditBtn = document.getElementById('cancelEdit');
let isEditing = false;
let currentProductId = null;

// Event Listeners
document.addEventListener('DOMContentLoaded', () => {
    loadProducts();
});

productForm.addEventListener('submit', handleFormSubmit);
searchInput.addEventListener('input', debounce(handleSearch, 300));
cancelEditBtn.addEventListener('click', resetForm);

// Functions
async function loadProducts() {
    try {
        const response = await fetch(API_BASE_URL);
        const products = await response.json();
        renderProducts(products);
    } catch (error) {
        console.error('Error loading products:', error);
        showError('Erro ao carregar produtos');
    }
}

async function handleSearch() {
    const searchTerm = searchInput.value.trim();
    if (searchTerm.length === 0) {
        loadProducts();
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/search?term=${encodeURIComponent(searchTerm)}`);
        const products = await response.json();
        renderProducts(products);
    } catch (error) {
        console.error('Error searching products:', error);
        showError('Erro ao buscar produtos');
    }
}

// Format price for display (BRL format)
function formatPrice(price) {
    if (price === null || price === undefined) return '0,00';
    return parseFloat(price).toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// Parse price from BRL format to number
function parsePrice(priceStr) {
    // Remove all non-numeric characters except comma and dot
    const cleaned = priceStr.replace(/[^0-9,.]/g, '');
    // Replace comma with dot and parse as float
    return parseFloat(cleaned.replace(/\./g, '').replace(',', '.'));
}

// Apply currency mask to input
function applyCurrencyMask(input) {
    let value = input.value.replace(/\D/g, '');
    value = (value / 100).toFixed(2) + '';
    value = value.replace(/\./, ',');
    value = value.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
    input.value = value;
}

async function handleFormSubmit(e) {
    e.preventDefault();
    
    const formData = new FormData(productForm);
    const priceValue = document.getElementById('price').value;
    
    const productData = {
        code: formData.get('code'),
        name: formData.get('name'),
        description: formData.get('description'),
        price: parsePrice(priceValue),
        isActive: document.getElementById('isActive').checked
    };

    try {
        if (isEditing) {
            await updateProduct(currentProductId, productData);
        } else {
            await createProduct(productData);
        }
        resetForm();
        loadProducts();
    } catch (error) {
        console.error('Error saving product:', error);
        showError('Erro ao salvar produto');
    }
}

async function createProduct(productData) {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(productData)
    });

    if (!response.ok) {
        throw new Error('Failed to create product');
    }

    showSuccess('Produto criado com sucesso!');
}

async function updateProduct(id, productData) {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ id, ...productData })
    });

    if (!response.ok) {
        throw new Error('Failed to update product');
    }

    showSuccess('Produto atualizado com sucesso!');
}

async function deleteProduct(id) {
    try {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error('Failed to delete product');
        }


        loadProducts();
        showSuccess('Produto removido com sucesso!');
    } catch (error) {
        console.error('Error deleting product:', error);
        showError('Erro ao remover produto');
    }
}

function renderProducts(products) {
    productTableBody.innerHTML = '';
    
    if (products.length === 0) {
        productTableBody.innerHTML = `
            <tr>
                <td colspan="5" class="px-6 py-4 text-center text-sm text-gray-500">
                    Nenhum produto encontrado
                </td>
            </tr>`;
        return;
    }


    products.forEach(product => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td class="px-6 py-4 whitespace-nowrap">${product.code}</td>
            <td class="px-6 py-4 whitespace-nowrap">
                <div class="font-medium text-gray-900">${product.name}</div>
                <div class="text-xs text-gray-500 line-clamp-2">${product.description || 'Sem descrição'}</div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                R$ ${formatPrice(product.price)}
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="${product.isActive ? 'status-active' : 'status-inactive'}">
                    ${product.isActive ? 'Ativo' : 'Inativo'}
                </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <button onclick="editProduct('${product.id}')" 
                        class="btn-edit">
                    Editar
                </button>
                <button onclick="confirmDelete(${product.id}, '${product.name.replace(/'/g, "\\'")}')" 
                        class="btn-delete">
                    Excluir
                </button>
            </td>
        `;
        productTableBody.appendChild(row);
    });
}

async function editProduct(productId) {
    try {
        const response = await fetch(`${API_BASE_URL}/${productId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch product');
        }
        const product = await response.json();
        
        isEditing = true;
        currentProductId = product.id;
        
        document.getElementById('formTitle').textContent = 'Editar Produto';
        document.getElementById('productId').value = product.id;
        document.getElementById('code').value = product.code;
        document.getElementById('name').value = product.name;
        document.getElementById('description').value = product.description || '';
        document.getElementById('price').value = formatPrice(product.price);
        document.getElementById('isActive').checked = product.isActive;
        
        cancelEditBtn.classList.remove('hidden');
        window.scrollTo({ top: 0, behavior: 'smooth' });
    } catch (error) {
        console.error('Error fetching product:', error);
        showError('Erro ao carregar produto para edição');
    }
}

function confirmDelete(id, name) {
    Swal.fire({
        title: 'Tem certeza?',
        text: `Deseja realmente remover o produto "${name}"?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, remover!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteProduct(id);
        }
    });
}

function resetForm() {
    isEditing = false;
    currentProductId = null;
    productForm.reset();
    document.getElementById('formTitle').textContent = 'Adicionar Novo Produto';
    cancelEditBtn.classList.add('hidden');
}

// Utility Functions
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function showSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: message,
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });
}

function showError(message) {
    Swal.fire({
        icon: 'error',
        title: 'Erro!',
        text: message,
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });
}

// Make functions available globally
window.editProduct = editProduct;
window.confirmDelete = confirmDelete;
