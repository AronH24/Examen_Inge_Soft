<template>
  <div class="container my-4">
    <h1 class="mb-4">Máquina Expendedora de Refrescos</h1>

    <table class="table table-bordered">
      <thead class="table-light">
        <tr>
          <th>Refrescos</th>
          <th>Cantidad (latas)</th>
          <th>Precio (colones)</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="drink in drinks" :key="drink.name">
          <td>{{ drink.name }}</td>
          <td>{{ drink.quantity }}</td>
          <td>{{ drink.price }}</td>
        </tr>
      </tbody>
    </table>

    <div class="row align-items-end mb-3">
      <div class="col-md-5">
        <label class="form-label">Refresco</label>
        <select v-model="selectedDrink" class="form-select">
          <option disabled value="">Seleccione un refresco</option>
          <option v-for="drink in drinks" :key="drink.name" :value="drink.name" :disabled="drink.quantity === 0">
            {{ drink.name }}
          </option>
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">Cantidad</label>
        <input type="number" class="form-control" v-model.number="selectedQuantity" min="1"/>
      </div>
      <div class="col-md-3">
        <button class="btn btn-primary w-100" @click="addToCart">Agregar</button>
      </div>
    </div>

    <div class="mb-3">
      <h5>Total = {{ total }} colones</h5>
    </div>

    <div class="row align-items-end mb-3">
      <div class="col-md-5">
        <label class="form-label">Insertar dinero</label>
        <select v-model.number="selectedMoney" class="form-select">
          <option disabled value="">Seleccione moneda o billete</option>
          <option v-for="amount in moneyOptions" :key="amount" :value="amount">{{ amount }}</option>
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">Cantidad</label>
        <input type="number" class="form-control" v-model.number="moneyQuantity" min="1" />
      </div>
      <div class="col-md-3">
        <button class="btn btn-primary w-100" @click="addMoney">Agregar</button>
      </div>
    </div>

    <div class="d-grid">
      <button class="btn btn-warning" @click="pay">Pagar</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      drinks: [],
      selectedDrink: '',
      selectedQuantity: 1,
      cart: [],
      total: 0,
      moneyOptions: [1000, 500, 100, 50, 25],
      selectedMoney: '',
      moneyQuantity: 1,
      moneyInserted: [],
    };
  },
  mounted() {
    this.getDrinks();
  },
  methods: {
    async getDrinks() {
    try {
      const res = await axios.get('https://localhost:7037/api/Vending');
      this.drinks = res.data;
    } catch (e) {
      alert('Error al cargar los refrescos');
    }
  },
  addToCart() {
      if (!this.selectedDrink || this.selectedQuantity < 1) {
        return;
      }
      const drink = this.drinks.find(d => d.name === this.selectedDrink);
      if (!drink || drink.quantity < this.selectedQuantity) {
        alert('No hay suficientes latas');
        return;
      }
      const existing = this.cart.find(item => item.DrinkName === this.selectedDrink);
      if (existing) {
        existing.Quantity += this.selectedQuantity;
      } else {
        this.cart.push({ DrinkName: this.selectedDrink, Quantity: this.selectedQuantity });
      }
      this.total += drink.price * this.selectedQuantity;
      this.selectedDrink = '';
      this.selectedQuantity = 1;
    },
    addMoney() {
      if (!this.selectedMoney || this.moneyQuantity < 1) {
        return;
      }
      const existingMoney = this.moneyInserted.find(money => money.MoneyType === this.selectedMoney);
      if (existingMoney) {
        existingMoney.Quantity += this.moneyQuantity;
      } else {
        this.moneyInserted.push({MoneyType: this.selectedMoney,Quantity: this.moneyQuantity});
      }
      this.selectedMoney = '';
      this.moneyQuantity = 1;
    },
    async pay() {
      try {
        const res = await axios.post('https://localhost:7037/api/Vending', {
          Drinks: this.cart,
          MoneyInserted: this.moneyInserted
        });
        alert(res.data.message || 'La compra ha sido exitosa!!');
        this.cart = [];
        this.moneyInserted = [];
        this.total = 0;
        this.fetchDrinks();
      } catch (e) {
        alert(e.response?.data?.message || 'Error en la compra!!');
      }
    }
  }
};
</script>

<style scoped>
.table {
  margin-bottom: 0.5rem;
}

.container {
  font-size: 20px; 
}

.btn {
    font-size: 20px;
}

.form-select {
    font-size: 20px;
}
</style>