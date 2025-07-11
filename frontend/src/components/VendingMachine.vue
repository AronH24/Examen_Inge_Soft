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
        <button class="btn btn-primary w-100">Agregar</button>
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
        <button class="btn btn-primary w-100">Agregar</button>
      </div>
    </div>

    <div class="d-grid">
      <button class="btn btn-warning">Pagar</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      drinks: [
        { name: 'Coca Cola', price: 800, quantity: 10 },
        { name: 'Pepsi', price: 750, quantity: 8 },
        { name: 'Fanta', price: 950, quantity: 10 },
        { name: 'Sprite', price: 975, quantity: 15 },
      ],
      selectedDrink: '',
      selectedQuantity: 1,
      cart: [],
      total: 0,
      moneyOptions: [1000, 500, 100, 50, 25],
      selectedMoney: '',
      moneyQuantity: 1,
      moneyInserted: 0,
    };
  },
  computed: {
  },
  methods: {
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