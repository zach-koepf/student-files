<template>
  <div class="search-bar" v-show="loggedIn(this.userId)">
    <form @submit.prevent="retrievePublicResults">
      <div class="group">
        <svg class="icon" aria-hidden="true" viewBox="0 0 24 24">
          <g>
            <path
              d="M21.53 20.47l-3.66-3.66C19.195 15.24 20 13.214 20 11c0-4.97-4.03-9-9-9s-9 4.03-9 9 4.03 9 9 9c2.215 0 4.24-.804 5.808-2.13l3.66 3.66c.147.146.34.22.53.22s.385-.073.53-.22c.295-.293.295-.767.002-1.06zM3.5 11c0-4.135 3.365-7.5 7.5-7.5s7.5 3.365 7.5 7.5-3.365 7.5-7.5 7.5-7.5-3.365-7.5-7.5z"
            ></path>
          </g>
        </svg>
        <input
          id="front"
          name="keyword"
          placeholder="Search Public Cards"
          v-model="searchInput"
          type="search"
          class="input form-item"
        />
      </div>
    </form>
  </div>
</template>

<script>
import cardService from '../services/CardService.js'
export default {
  name: "public-cards-search-bar",
  data() {
    return {
      myUserId: this.$store.state.user.userId,
      searchInput: "",
    };
  },
  computed: {
    userId() {
      return this.$store.state.user.userId;
    },
  },
  methods: {
    loggedIn(userId) {
      if (userId > 0) {
        return true;
      } else {
        return false;
      }
    },
    retrievePublicResults() {
        cardService
          .getPublicCardSearchResults(this.searchInput)
          .then((response) => {
            if (response.status == 200) {
              this.$store.commit("SET_CARDS", response.data);
              this.$router.push({path: `/search/${this.searchInput}`});
            }
          })
          .catch((error) => {
            alert(error);
          });
      }
  },
};
</script>

<style>
/* .search-bar {
  display: flex;
  align-items: center;
  float: right;
} */

.form-item {
  margin: 10px;
}

.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 250px;
}

.input {
  width: 100%;
  height: 40px;
  line-height: 28px;
  padding: 0 1rem;
  padding-left: 2.5rem;
  border: 2px solid transparent;
  border-radius: 8px;
  outline: none;
  background-color: #fff;
  color: #0d0c22;
  transition: 0.3s ease;
}

.input::placeholder {
  color: #000;
}

.input:focus,
input:hover {
  outline: none;
  border-color: rgb(32 49 89 / 40%);
  background-color: #fff;
  box-shadow: 0 0 0 4px rgb(32 49 89 / 10%);
}

.icon {
  position: absolute;
  left: 1rem;
  fill: white;/*#9e9ea7;*/
  width: 1rem;
  height: 1rem;
}
</style>