<template>
  <div class="vending-machine">
    <h1>Máquina Expendedora de Refrescos</h1>
    <div class="drinks-list">
      <ul>
        <li v-for="drink in drinks" :key="drink.name">
          {{ drink.name }} {{ drink.price }} colones ({{ drink.quantity }} disponibles)
          <input type="number" v-model.number="drink.selectedQuantity" min="0" :max="drink.quantity" />
          <button>Agregar al Carrito</button>
        </li>
      </ul>
    </div>
    <div class="cart">
      <h2>Carrito de Compras</h2>
      <ul>
        <li v-for="item in cart" :key="item.name">
          {{ item.name }} {{ item.quantity }} unidades
        </li>
      </ul>
      <button>Pagar</button>
    </div>
    <div class="payment">
      <h2>Ingreso de Dinero</h2>
      <input type="number" v-model.number="moneyInserted" placeholder="Ingrese dinero por favor" />
      <button>Agregar Dinero</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      drinks: [
        { name: 'Coca Cola', price: 800, quantity: 10, selectedQuantity: 0 },
        { name: 'Pepsi', price: 750, quantity: 8, selectedQuantity: 0 },
        { name: 'Fanta', price: 950, quantity: 10, selectedQuantity: 0 },
        { name: 'Sprite', price: 975, quantity: 15, selectedQuantity: 0 },
      ],
      cart: [],
      moneyInserted: 0,
    };
  },
  computed: {
  },
  methods: {
  },
};
</script>

<style scoped>
.vending-machine {
  max-width: 600px;
  margin: auto;
  text-align: center;
}
.drinks-list, .cart, .payment {
  margin: 20px 0;
}
</style>