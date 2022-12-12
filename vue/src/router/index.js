import Vue from 'vue'
import Router from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import Logout from '../views/Logout.vue'
import Register from '../views/Register.vue'
import store from '../store/index'
import AccountPage from '../views/AccountPage.vue'
import DeckEditor from '../views/DeckEditor.vue'
import StudySession from '../views/StudySession.vue'
<<<<<<< HEAD
import SearchCards from '../components/SearchCards.vue'
import TESTStudySession from '../views/TESTStudySession.vue'
// import Results from '../views/Results.vue'
=======
import Results from '../views/Results.vue'
import Public from '../views/Public.vue'
>>>>>>> 59ab13f9b1cce837c47dc78873e0d815c25f22f0

Vue.use(Router)

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',// changed this to false does not require auth
      name: 'homepage',
      component: Home,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/logout",
      name: "logout",
      component: Logout,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/register",
      name: "register",
      component: Register,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/deck",
      name: "myDecks",
      component: AccountPage,
      meta:{
        requiresAuth: true
      }
    },
    {
      path: "/deck/:deckId/card",
      name: "DeckEditor",
      component: DeckEditor,
      meta:{
        requiresAuth: true
      }
    },
    {
      path: '/card', // this route was '/study'
      name: 'StudySession',
      component: StudySession,
      meta:{
        requiresAuth: true
      }
    },
    {
      path: '/cardTEST', // Joe: added a test route, will delete this once things are working properly
      name: 'TESTStudySession',
      component: TESTStudySession,
      meta:{
        requiresAuth: true
      }
    },
    {
      path: '/card/keyword/:searchInput',
      name: 'SearchCards',
      component: SearchCards,
      // path: '/search',
      // name: 'results',
      // component: Results,
      meta:{
        requiresAuth: true
      }
    },
    {
      path: '/public',
      name: 'public',
      component: Public,
      meta:{
        requiresAuth: false
      }
    },
    // {
    //   path: '/deck/:deckId/card/create',
    //   name: 'CardForm',
    //   component: CardForm,
    //   meta:{
    //     requiresAuth: true
    //   }
    // },
    // {
    //   path: '/deck/:deckId/card/:cardId/edit',
    //   name: 'EditCard',
    //   component: EditCard
    // }
  ]
})

router.beforeEach((to, from, next) => {
  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && store.state.token === '') {
    next("/login");
  } else {
    // Else let them go to their next destination
    next();
  }
});

export default router;
