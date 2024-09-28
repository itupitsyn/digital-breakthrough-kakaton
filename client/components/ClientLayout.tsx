"use client";

import { FC, PropsWithChildren } from "react";
import { UltraNavbar } from "./UltraNavbar";
import { SearchProvider } from "@/contexts/searchContext";
import { AuthProvider } from "@/contexts/authContext";
import { Toaster } from "react-hot-toast";
import { TransitionContextProvider } from "@/contexts/transitionContext";

export const ClientLayout: FC<PropsWithChildren> = ({ children }) => {
  return (
    <TransitionContextProvider>
      <SearchProvider>
        <AuthProvider>
          <div className="container min-h-svh pb-12">
            <UltraNavbar />
            {children}
            <Toaster />
          </div>
        </AuthProvider>
      </SearchProvider>
    </TransitionContextProvider>
  );
};
