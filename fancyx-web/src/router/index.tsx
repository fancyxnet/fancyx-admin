import { type RouteObject } from 'react-router-dom';
import Login from '@/pages/auth/login.tsx';
import Layout from '@/layout';
import NotFound from '@/pages/error/notFound';
import Home from '@/pages/home';
import Profile from '@/pages/auth/profile.tsx';
import Authorize from '@/components/Authorize';
import { StaticRoutes } from '@/utils/globalValue.ts';
import ExternalWrapper from '@/components/ExternalWrapper';

const routes: RouteObject[] = [
  {
    path: StaticRoutes.Login,
    element: <Login />,
  },
  {
    path: '/',
    element: (
      <Authorize>
        <Layout />
      </Authorize>
    ),
    children: [
      {
        path: '',
        element: <Home />,
      },
      {
        path: '*',
        element: <NotFound />,
      },
      {
        path: '/profile',
        element: <Profile />,
      },
      {
        path: '/external/*',
        element: <ExternalWrapper />,
      },
    ],
  },
];

const needDisplayRoutes = [
  {
    path: '/profile',
    title: '个人中心',
  }
]

export { routes, needDisplayRoutes };
