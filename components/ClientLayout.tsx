"use client";

import { FC, PropsWithChildren } from "react";
import { UltraNavbar } from "./UltraNavbar";

export const ClientLayout: FC<PropsWithChildren> = ({ children }) => {
  return (
    <div className="container min-h-svh pb-12">
      <UltraNavbar />
      {children}
    </div>
  );
};
