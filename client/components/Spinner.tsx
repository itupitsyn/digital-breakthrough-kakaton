import classNames from "classnames";
import { FC } from "react";
import { ImSpinner4 } from "react-icons/im";

interface SpinnerProps {
  className?: string;
}

export const Spinner: FC<SpinnerProps> = ({ className }) => {
  return (
    <div className={classNames("absolute inset-0 flex justify-center backdrop-blur-sm", className)}>
      <ImSpinner4 className="mt-10 size-12 animate-spin text-fuchsia-500" />
    </div>
  );
};
