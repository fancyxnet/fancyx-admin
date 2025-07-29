import { App } from 'antd';
import React, { createContext, useContext, useEffect } from 'react';
import UserStore from '@/store/userStore';

export interface ApplicationContextType {
  ossDomain: string;
}

const ApplicationContext = createContext<ApplicationContextType>({ ossDomain: '' });
const Application = ({ children }: { children: React.ReactNode }) => {
  const ossDomain = import.meta.env.VITE_OSS_DOMAIN;

  useEffect(() => {
    if (UserStore.isAuthenticated()) {
      UserStore.startTokenChecker();
    }

    return () => {
      UserStore.stopTokenChecker();
    };
  }, []);

  return (
    <ApplicationContext.Provider value={{ ossDomain }}>
      <App>{children}</App>
    </ApplicationContext.Provider>
  );
};

export default Application;

// eslint-disable-next-line react-refresh/only-export-components
export const useApplication = () => useContext(ApplicationContext);
